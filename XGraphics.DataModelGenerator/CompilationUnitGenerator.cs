using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XGraphics.DataModelGenerator
{
    public class CompilationUnitGenerator
    {
        private readonly InterfaceDeclarationSyntax _sourceInterfaceDeclaration;
        private readonly string _rootDirectory;
        private readonly OutputType _outputType;
        private readonly string _interfaceName;
        private readonly IdentifierNameSyntax _destinationClassName;
        private readonly NameSyntax _sourceNamespaceName;
        private readonly CompilationUnitSyntax _sourceCompilationUnit;
        private readonly QualifiedNameSyntax _destinationNamespaceName;

        public CompilationUnitGenerator(InterfaceDeclarationSyntax sourceInterfaceDeclaration, string rootDirectory, OutputType outputType)
        {
            _sourceInterfaceDeclaration = sourceInterfaceDeclaration;
            _rootDirectory = rootDirectory;
            _outputType = outputType;

            _interfaceName = _sourceInterfaceDeclaration.Identifier.Text;
            if (!_interfaceName.StartsWith("I"))
                throw new UserViewableException($"Data model interface {_interfaceName} must start with 'I'");

            _destinationClassName = IdentifierName(_interfaceName.Substring(1));

            if (!(_sourceInterfaceDeclaration.Parent is NamespaceDeclarationSyntax interfaceNamespaceDeclaration))
                throw new UserViewableException(
                    $"Parent of ${_interfaceName} interface should be namespace declaration, but it's a {_sourceInterfaceDeclaration.Parent.GetType()} node instead");
            _sourceNamespaceName = interfaceNamespaceDeclaration.Name;

            if (!(interfaceNamespaceDeclaration.Parent is CompilationUnitSyntax compilationUnit))
                throw new UserViewableException(
                    $"Parent of ${interfaceNamespaceDeclaration} namespace should be compilation unit, but it's a {interfaceNamespaceDeclaration.Parent.GetType()} node instead");
            _sourceCompilationUnit = compilationUnit;

            _destinationNamespaceName = ToDestinationNamespaceName(_sourceNamespaceName);
        }

        public void Generate()
        {
            bool hasChildrenProperty = false;

            var destinationStaticMembers = new List<MemberDeclarationSyntax>();
            var destinationMembers = new List<MemberDeclarationSyntax>();
            var collectionProperties = new List<PropertyDeclarationSyntax>();
            foreach (MemberDeclarationSyntax modelObjectMember in _sourceInterfaceDeclaration.Members)
            {
                if (!(modelObjectMember is PropertyDeclarationSyntax modelProperty))
                    continue;

                string propertyName = modelProperty.Identifier.Text;
                string propertyDescriptorName = propertyName + "Property";
                TypeSyntax destinationPropertyType = ToDestinationType(modelProperty.Type);
                ExpressionSyntax defaultValue = GetDefaultValue(modelProperty);

                bool isCollection = IsCollectionType(modelProperty.Type, out TypeSyntax collectionElementType);
                if (isCollection)
                    collectionProperties.Add(modelProperty);
                else AddPropertyDescriptor(propertyName, propertyDescriptorName, destinationPropertyType, defaultValue, destinationStaticMembers);

                AddProperty(propertyName, propertyDescriptorName, destinationPropertyType, modelProperty.Type, destinationMembers);

                if (propertyName == "Children")
                    hasChildrenProperty = true;
            }

            // Add an annotation on the last static member, so we can later add a blank line between it and the properties that follow
            var lastStaticMemberAnnotation = new SyntaxAnnotation();
            int staticMemberCount = destinationStaticMembers.Count;
            if (staticMemberCount > 0)
                destinationStaticMembers[staticMemberCount - 1] = destinationStaticMembers[staticMemberCount - 1].WithAdditionalAnnotations(lastStaticMemberAnnotation);

            ConstructorDeclarationSyntax? constructor = CreateConstructor(collectionProperties);

            TypeSyntax? baseInterface = _sourceInterfaceDeclaration.BaseList?.Types.FirstOrDefault()?.Type;
            TypeSyntax destinationBaseClass = GetBaseClass(baseInterface);

            List<MemberDeclarationSyntax> classMembers = new List<MemberDeclarationSyntax>();
            classMembers.AddRange(destinationStaticMembers);
            if (constructor != null)
                classMembers.Add(constructor);
            classMembers.AddRange(destinationMembers);

            var classDeclaration =
                ClassDeclaration(_destinationClassName.Identifier)
                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                    .WithBaseList(
                        BaseList(
                            SeparatedList<BaseTypeSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SimpleBaseType(destinationBaseClass),
                                    Token(SyntaxKind.CommaToken),
                                    SimpleBaseType(IdentifierName(_interfaceName))
                                })))
                    .WithMembers(new SyntaxList<MemberDeclarationSyntax>(classMembers));

            if (DestinationTypeHasTypeConverterAttribute)
            {
                classDeclaration =
                    classDeclaration.WithAttributeLists(
                        SingletonList(
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(
                                            IdentifierName("TypeConverter"))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList(
                                                    AttributeArgument(
                                                        TypeOfExpression(
                                                            IdentifierName($"{_destinationClassName.Identifier}TypeConverter"))))))))));
            }

            // Add the [ContentProperty("Children")] attribute, if needed
            if (hasChildrenProperty)
                classDeclaration = classDeclaration
                    .WithAttributeLists(
                        SingletonList(
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(
                                            IdentifierName("ContentProperty"))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList(
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            Literal("Children"))))))))));

#if false
            var usings = new List<UsingDirectiveSyntax>
            {
                UsingDirective(_interfaceNamespaceName)
                    .WithUsingKeyword(
                        Token(
                            TriviaList(
                                Comment($"// This file is generated from {_interfaceName}.cs. Update the source file to change its contents."),
                                CarriageReturnLineFeed),
                            SyntaxKind.UsingKeyword,
                            TriviaList())),
                UsingDirective(QualifiedName(IdentifierName("System"), IdentifierName("Windows")))
            };
            if (addCollectionsUsing)
            {
                usings.Add(UsingDirective(QualifiedName(
                    QualifiedName(IdentifierName("System"), IdentifierName("Collections")),
                    IdentifierName("Generic"))));
            }
            if (addTransformsUsing)
            {
                usings.Add(UsingDirective(QualifiedName(IdentifierName("XGraphics"), IdentifierName("Transforms"))));
                // This will be, for example, XGraphics.WPF.Transforms
                usings.Add(UsingDirective(QualifiedName(_destinationNamespaceName, IdentifierName("Transforms"))));
            }
            if (hasChildrenProperty)
            {
                usings.Add(UsingDirective(QualifiedName(
                    QualifiedName(IdentifierName("System"), IdentifierName("Windows")),
                    IdentifierName("Markup"))));
            }
#endif
            SyntaxList<UsingDirectiveSyntax> usingDeclarations = CreateUsingDeclarations(destinationStaticMembers.Count > 0);

            var compilationUnit =
                CompilationUnit()
                    .WithUsings(usingDeclarations)
                    .WithMembers(
                        SingletonList<MemberDeclarationSyntax>(
                            NamespaceDeclaration(_destinationNamespaceName)
                                .WithMembers(SingletonList<MemberDeclarationSyntax>(classDeclaration))))
                    .NormalizeWhitespace();

            MemberDeclarationSyntax? lastStaticMember = compilationUnit.DescendantNodes()
                .OfType<MemberDeclarationSyntax>().FirstOrDefault(n => n.HasAnnotation(lastStaticMemberAnnotation));
            if (lastStaticMember != null)
            {
                var newTrailingTrivia = lastStaticMember.GetTrailingTrivia().Add(CarriageReturnLineFeed);
                compilationUnit = compilationUnit.ReplaceNode(lastStaticMember,
                    lastStaticMember.WithTrailingTrivia(newTrailingTrivia));
            }

            SourceText destinationSourceText = compilationUnit.GetText();

            string outputDirectory = GetOutputDirectory(_sourceNamespaceName);
            Directory.CreateDirectory(outputDirectory);

            string destinationFilePath = Path.Combine(outputDirectory, _destinationClassName + ".cs");
            using (StreamWriter fileWriter = File.CreateText(destinationFilePath))
                destinationSourceText.Write(fileWriter);
        }

        private ConstructorDeclarationSyntax? CreateConstructor(List<PropertyDeclarationSyntax> collectionProperties)
        {
            if (collectionProperties.Count == 0)
                return null;

            List<StatementSyntax> statements = new List<StatementSyntax>();
            foreach (PropertyDeclarationSyntax property in collectionProperties)
            {
                string propertyName = property.Identifier.Text;
                TypeSyntax destinationPropertyType = ToDestinationType(property.Type);

                statements.Add(
                    ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(propertyName),
                            ObjectCreationExpression(
                                    destinationPropertyType)
                                .WithArgumentList(
                                    ArgumentList()))));

                statements.Add(
                ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.AddAssignmentExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName(propertyName),
                                IdentifierName("Changed")),
                            IdentifierName("OnSubobjectChanged"))));
            }

            return ConstructorDeclaration(_destinationClassName.Identifier)
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithBody(
                    Block(
                        statements));
        }

        private void AddPropertyDescriptor(string propertyName, string propertyDescriptorName, TypeSyntax propertyType,
            ExpressionSyntax defaultValue, List<MemberDeclarationSyntax> destinationStaticMembers)
        {
            if (!(_outputType is XamlOutputType xamlOutputType))
                return;

            TypeSyntax nonNullablePropertyType;
            if (propertyType is NullableTypeSyntax nullablePropertyType)
                nonNullablePropertyType = nullablePropertyType.ElementType;
            else nonNullablePropertyType = propertyType;

            var propertyDescriptor =
                    FieldDeclaration(
                            VariableDeclaration(xamlOutputType.DependencyPropertyClassName)
                                .WithVariables(
                                    SingletonSeparatedList(
                                        VariableDeclarator(Identifier(propertyDescriptorName))
                                            .WithInitializer(
                                                EqualsValueClause(
                                                    InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName("PropertyUtils"),
                                                                IdentifierName("Create")))
                                                        .WithArgumentList(
                                                            ArgumentList(
                                                                SeparatedList<ArgumentSyntax>(
                                                                    new SyntaxNodeOrToken[]
                                                                    {
                                                                        Argument(
                                                                            InvocationExpression(
                                                                                    IdentifierName("nameof"))
                                                                                .WithArgumentList(
                                                                                    ArgumentList(
                                                                                        SingletonSeparatedList(
                                                                                            Argument(IdentifierName(
                                                                                                propertyName)))))),
                                                                        Token(SyntaxKind.CommaToken),
                                                                        Argument(TypeOfExpression(nonNullablePropertyType)),
                                                                        Token(SyntaxKind.CommaToken),
                                                                        Argument(TypeOfExpression(_destinationClassName)),
                                                                        Token(SyntaxKind.CommaToken),
                                                                        Argument(defaultValue)
                                                                    }))))))))
                        .WithModifiers(
                            TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.ReadOnlyKeyword)));

            destinationStaticMembers.Add(propertyDescriptor);
        }

        private void AddProperty(string propertyName, string propertyDescriptorName, TypeSyntax propertyType, TypeSyntax sourcePropertyType,
            List<MemberDeclarationSyntax> destinationMembers)
        {
            IdentifierNameSyntax propertyDescriptorIdentifier = IdentifierName(propertyDescriptorName);

            if (! ReferenceEquals(sourcePropertyType, propertyType))
            {
                ExpressionSyntax arrowRightHandSide;
                if (sourcePropertyType is IdentifierNameSyntax identifierName &&
                    IsWrappedType(identifierName.Identifier.Text))
                {
                    string wrapperTypeName = identifierName.Identifier.Text;

                    arrowRightHandSide =
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(propertyName),
                            IdentifierName($"Wrapped{wrapperTypeName}"));
                }
                else arrowRightHandSide = IdentifierName(propertyName);

                var explicitInterfaceProperty =
                    PropertyDeclaration(sourcePropertyType, propertyName)
                        .WithExplicitInterfaceSpecifier(
                            ExplicitInterfaceSpecifier(IdentifierName(_sourceInterfaceDeclaration.Identifier)))
                        .WithExpressionBody(
                            ArrowExpressionClause(arrowRightHandSide))
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

                destinationMembers.Add(explicitInterfaceProperty);
            }

            PropertyDeclarationSyntax propertyDeclaration;
            if (IsCollectionType(sourcePropertyType, out TypeSyntax collectionElementType))
            {
                propertyDeclaration =
                    PropertyDeclaration(propertyType, propertyName)
                        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                        .WithAccessorList(
                            AccessorList(
                                SingletonList(
                                    AccessorDeclaration(
                                            SyntaxKind.GetAccessorDeclaration)
                                        .WithSemicolonToken(
                                            Token(SyntaxKind.SemicolonToken)))));
            }
            else
            {
                propertyDeclaration = PropertyDeclaration(propertyType, propertyName)
                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                    .WithAccessorList(
                        AccessorList(
                            List(new[]
                            {
                                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                    .WithExpressionBody(
                                        ArrowExpressionClause(
                                            CastExpression(
                                                propertyType,
                                                InvocationExpression(IdentifierName("GetValue"))
                                                    .WithArgumentList(ArgumentList(
                                                        SingletonSeparatedList(
                                                            Argument(propertyDescriptorIdentifier)))))))
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                    .WithExpressionBody(
                                        ArrowExpressionClause(InvocationExpression(IdentifierName("SetValue"))
                                            .WithArgumentList(ArgumentList(
                                                SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]
                                                    {
                                                        Argument(propertyDescriptorIdentifier),
                                                        Token(SyntaxKind.CommaToken),
                                                        Argument(IdentifierName("value"))
                                                    })))))
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                            })));
            }

            destinationMembers.Add(propertyDeclaration);
        }

        private SyntaxList<UsingDirectiveSyntax> CreateUsingDeclarations(bool hasPropertyDescriptors)
        {
            var usingNames = new Dictionary<string, NameSyntax>();

            foreach (UsingDirectiveSyntax sourceUsing in _sourceCompilationUnit.Usings)
            {
                NameSyntax sourceUsingName = sourceUsing.Name;
                AddUsing(usingNames, sourceUsingName);

                if (sourceUsingName.ToString().StartsWith("XGraphics."))
                    AddUsing(usingNames, ToDestinationNamespaceName(sourceUsingName));
            }

            AddUsing(usingNames, _sourceNamespaceName);

            IEnumerable<QualifiedNameSyntax> outputTypeUsings = _outputType.GetUsings(hasPropertyDescriptors, DestinationTypeHasTypeConverterAttribute);
            foreach (QualifiedNameSyntax outputTypeUsing in outputTypeUsings)
                AddUsing(usingNames, outputTypeUsing);

            if (DestinationTypeHasTypeConverterAttribute)
                AddUsing(usingNames, QualifiedName(_outputType.RootNamespace, IdentifierName("Converters")));

            var usingDirectives = new List<UsingDirectiveSyntax>();
            foreach (NameSyntax name in usingNames.Values)
                usingDirectives.Add(UsingDirective(name));

            return new SyntaxList<UsingDirectiveSyntax>(usingDirectives);
        }

        private static void AddUsing(Dictionary<string, NameSyntax> usingNames, NameSyntax name)
        {
            string usingString = name.ToString();
            if (! usingNames.ContainsKey(usingString))
                usingNames.Add(usingString, name);
        }

        private QualifiedNameSyntax ToDestinationNamespaceName(NameSyntax sourceNamespaceName)
        {
            if (!sourceNamespaceName.ToString().StartsWith("XGraphics"))
                throw new InvalidOperationException($"Source namespace {sourceNamespaceName} doesn't start with 'XGraphics.' as expected");

            // Map e.g. XGraphics.Shapes source namespace => XGraphics.WPF.Shapes destination namespace
            QualifiedNameSyntax destinationNamespaceName = _outputType.RootNamespace;
            if (sourceNamespaceName is QualifiedNameSyntax qualifiedName)
                destinationNamespaceName = QualifiedName(destinationNamespaceName, qualifiedName.Right);

            return destinationNamespaceName;
        }

        private static TypeSyntax ToDestinationType(TypeSyntax sourceType)
        {
            if (IsCollectionType(sourceType, out TypeSyntax elementType))
            {
                if (IsPointType(elementType))
                    return IdentifierName("PointCollection");
                else
                {
                    TypeSyntax elementDestinationType = ToDestinationType(elementType);
                    return GenericName(
                            Identifier("GraphicsObjectCollection"))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SingletonSeparatedList(elementDestinationType)));
                }
            }
            else if (sourceType is IdentifierNameSyntax identifierName)
                return GetIdentifierDestinationType(identifierName);
            else if (sourceType is NullableTypeSyntax nullableType &&
                     nullableType.ElementType is IdentifierNameSyntax nullableIdentifierName)
                return NullableType(GetIdentifierDestinationType(nullableIdentifierName));
            else if (sourceType is PredefinedTypeSyntax predefinedType)
                return predefinedType;
            else if (sourceType is GenericNameSyntax genericName)
                return genericName;
            else if (sourceType is NameSyntax name)
                return name;
            /*
                PropertyDeclaration(
                    GenericName(
                        Identifier("IEnumerable"))
                    .WithTypeArgumentList(
                        TypeArgumentList(
                            SingletonSeparatedList<TypeSyntax>(
                                IdentifierName("IGraphicsElement")))),
                    Identifier("Children"))             */
            else
                throw new UserViewableException(
                    $"Type {sourceType.GetType()} isn't supported for model object generation");
        }

        private static NameSyntax GetIdentifierDestinationType(IdentifierNameSyntax identifierName)
        {
            string typeName = identifierName.Identifier.Text;
            if (typeName.StartsWith("I"))
                return IdentifierName(typeName.Substring(1));
            else if (IsEnumType(typeName))
                return identifierName;
            else if (IsWrappedType(typeName))
                return QualifiedName(IdentifierName("Wrapper"), IdentifierName(typeName));
            else
                throw new UserViewableException(
                    $"Identifier type {typeName} isn't supported for model object generation; interface name starting with 'I' is expected");
        }

        private bool DestinationTypeHasTypeConverterAttribute => _destinationClassName.Identifier.Text == "Geometry" && _outputType is XamlOutputType;

        private static bool IsTransformType(TypeSyntax type)
        {
            if (type is NullableTypeSyntax nullableType)
                type = nullableType.ElementType;

            return type is IdentifierNameSyntax identifierName && identifierName.Identifier.Text.EndsWith("Transform");
        }

        private static bool IsWrappedType(string typeName)
        {
            return typeName == "Color" || typeName == "Point" || typeName == "Size";
        }

        private static bool IsEnumType(string typeName)
        {
            return typeName == "SweepDirection" || typeName == "FillRule";
        }

        private static bool IsPointType(TypeSyntax type)
        {
            if (!(type is IdentifierNameSyntax identifierName))
                return false;

            return identifierName.Identifier.Text == "Point";
        }

        private static bool IsCollectionType(TypeSyntax type, out TypeSyntax elementType)
        {
            elementType = IdentifierName("INVALID");
            if (!(type is GenericNameSyntax genericName))
                return false;

            if (genericName.Identifier.Text != "IEnumerable")
                return false;

            if (!(genericName.TypeArgumentList.Arguments.Count == 1 &&
                  genericName.TypeArgumentList.Arguments[0] is IdentifierNameSyntax elementIdentifierName))
                throw new InvalidOperationException($"Type {genericName} doesn't have a single identifier generic argument as expected");

            elementType = elementIdentifierName;
            return true;
        }

        private static ExpressionSyntax GetDefaultValue(PropertyDeclarationSyntax modelProperty)
        {
            foreach (AttributeListSyntax attributeList in modelProperty.AttributeLists)
            {
                foreach (AttributeSyntax attribute in attributeList.Attributes)
                {
                    if (attribute.Name.ToString() != "ModelDefaultValue")
                        continue;

                    AttributeArgumentSyntax? firstArgument = attribute.ArgumentList.Arguments.FirstOrDefault();
                    if (firstArgument == null)
                        throw new UserViewableException($"Property {modelProperty.Identifier.Text} should have an argument for the [ModelDefaultValue] attribute");

                    return firstArgument.Expression;
                }
            }

            TypeSyntax propertyType = modelProperty.Type;
            if (propertyType is GenericNameSyntax genericName && genericName.Identifier.Text == "IEnumerable" &&
                genericName.TypeArgumentList.Arguments.Count == 1 &&
                genericName.TypeArgumentList.Arguments[0] is IdentifierNameSyntax elementIdentifierName)
            {
                return
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("PropertyUtils"),
                            GenericName(
                                    Identifier("Empty"))
                                .WithTypeArgumentList(
                                    TypeArgumentList(SingletonSeparatedList<TypeSyntax>(elementIdentifierName)))));
            }
            else if (propertyType is IdentifierNameSyntax propertyTypeName && (propertyTypeName.Identifier.Text == "Color" ||
                                                                               propertyTypeName.Identifier.Text == "Point" ||
                                                                               propertyTypeName.Identifier.Text == "Size"))
            {
                return
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("PropertyUtils"),
                        IdentifierName($"Default{propertyTypeName.Identifier.Text}"));
            }

            throw new UserViewableException($"Property {modelProperty.Identifier.Text} has no [ModelDefaultValue] attribute nor hardcoded default");
        }

        private TypeSyntax GetBaseClass(TypeSyntax? baseInterface)
        {
            if (baseInterface == null)
                return _outputType.BaseClassName;
            else
                return ToDestinationType(baseInterface);
        }

        private string GetOutputDirectory(NameSyntax namespaceName)
        {
            string outputDirectory = Path.Combine(_rootDirectory, _outputType.ProjectDirectory);
            string? childNamespace = GetChildNamespace(namespaceName);
            if (childNamespace != null)
                outputDirectory = Path.Combine(outputDirectory, childNamespace);

            return outputDirectory;
        }

        /// <summary>
        /// Return the child namespace (e.g. "Shapes", "Transforms", etc. or null if there is no child
        /// and classes should be at the root.
        /// </summary>
        /// <param name="namespaceName">source namespace</param>
        /// <returns>child namespace</returns>
        private static string? GetChildNamespace(NameSyntax namespaceName)
        {
            string namespaceNameString = namespaceName.ToString();

            int periodIndex = namespaceNameString.IndexOf('.');
            if (periodIndex == -1)
                return null;
            else return namespaceNameString.Substring(periodIndex + 1);
        }
    }
}