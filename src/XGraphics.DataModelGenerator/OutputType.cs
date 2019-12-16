using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace XGraphics.DataModelGenerator
{
    public abstract class OutputType
    {
        public abstract string ProjectBaseDirectory { get; }
        public abstract QualifiedNameSyntax RootNamespace { get; }
        public abstract IdentifierNameSyntax? BaseClassName { get; }
        public abstract IEnumerable<QualifiedNameSyntax> GetUsings(bool hasPropertyDescriptors, bool hasTypeConverterAttribute);
        public abstract bool EmitChangedNotifications { get; }
    }

    public abstract class XamlOutputType : OutputType
    {
        public abstract IdentifierNameSyntax DependencyPropertyClassName { get; }

        public override bool EmitChangedNotifications => true;
    }

    public class WpfXamlOutputType : XamlOutputType
    {
        public static readonly WpfXamlOutputType Instance = new WpfXamlOutputType();

        public override string ProjectBaseDirectory => "XGraphics.WPF";
        public override QualifiedNameSyntax RootNamespace =>
            QualifiedName(IdentifierName("XGraphics"), IdentifierName("WPF"));
        public override IdentifierNameSyntax DependencyPropertyClassName => IdentifierName("DependencyProperty");
        public override IdentifierNameSyntax? BaseClassName => IdentifierName("DependencyObjectWithCascadingNotifications");

        public override IEnumerable<QualifiedNameSyntax> GetUsings(bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
            var usings = new List<QualifiedNameSyntax>();

            if (hasPropertyDescriptors)
                usings.Add(QualifiedName(IdentifierName("System"), IdentifierName("Windows")));
            if (hasTypeConverterAttribute)
                usings.Add(QualifiedName(IdentifierName("System"), IdentifierName("ComponentModel")));

            usings.Add(QualifiedName(
                    QualifiedName(IdentifierName("System"), IdentifierName("Windows")),
                    IdentifierName("Markup")));

            return usings;
        }
    }

    public class UwpXamlOutputType : XamlOutputType
    {
        public static readonly UwpXamlOutputType Instance = new UwpXamlOutputType();

        public override string ProjectBaseDirectory => "XGraphics.UWP";
        public override QualifiedNameSyntax RootNamespace =>
            QualifiedName(IdentifierName("XGraphics"), IdentifierName("UWP"));
        public override IdentifierNameSyntax DependencyPropertyClassName => IdentifierName("DependencyProperty");
        public override IdentifierNameSyntax? BaseClassName => IdentifierName("DependencyObjectWithCascadingNotifications");
        public override IEnumerable<QualifiedNameSyntax> GetUsings(bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
            throw new NotImplementedException();
        }
    }

    public class XamarinFormsXamlOutputType : XamlOutputType
    {
        public static readonly XamarinFormsXamlOutputType Instance = new XamarinFormsXamlOutputType();

        public override string ProjectBaseDirectory => Path.Combine("XamarinForms", "XGraphics.XamarinForms");
        public override QualifiedNameSyntax RootNamespace =>
            QualifiedName(IdentifierName("XGraphics"), IdentifierName("XamarinForms"));
        public override IdentifierNameSyntax DependencyPropertyClassName => IdentifierName("BindableProperty");
        public override IdentifierNameSyntax? BaseClassName => IdentifierName("BindableObjectWithCascadingNotifications");

        public override IEnumerable<QualifiedNameSyntax> GetUsings(bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
            var usings = new List<QualifiedNameSyntax>();
            usings.Add(QualifiedName(IdentifierName("Xamarin"), IdentifierName("Forms")));
            return usings;
        }
    }

    public class StandardModelOutputType : OutputType
    {
        public static readonly StandardModelOutputType Instance = new StandardModelOutputType();

        public override string ProjectBaseDirectory => Path.Combine("XGraphics", "StandardModel");
        public override QualifiedNameSyntax RootNamespace =>
            QualifiedName(IdentifierName("XGraphics"), IdentifierName("StandardModel"));
        public override IdentifierNameSyntax? BaseClassName => IdentifierName("ObjectWithCascadingNotifications");

        public override IEnumerable<QualifiedNameSyntax> GetUsings(bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
            return new List<QualifiedNameSyntax>();
        }

        public override bool EmitChangedNotifications => false;
    }
}
