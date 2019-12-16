using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace XGraphics.DataModelGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Attempt to set the version of MSBuild.
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            var instance = visualStudioInstances.Length == 1
                // If there is only one instance of MSBuild on this machine, set that as the one to use.
                ? visualStudioInstances[0]
                // Handle selecting the version of MSBuild you want to use.
                : SelectVisualStudioInstance(visualStudioInstances);

            Console.WriteLine($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");

            // NOTE: Be sure to register an instance with the MSBuildLocator 
            //       before calling MSBuildWorkspace.Create()
            //       otherwise, MSBuildWorkspace won't MEF compose.
            MSBuildLocator.RegisterInstance(instance);

            using MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            // Print message for WorkspaceFailed event to help diagnosing project load failures.
            workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);

            if (args.Length < 1)
            {
                Console.WriteLine($"Usage: XGraphics.DataModelGenerator.exe <path-to-XGraphics-project>");
                Environment.Exit(1);
            }

            string xgraphicsProjectPath = args[0];
            Console.WriteLine($"Loading solution '{xgraphicsProjectPath}'");

            // Attach progress reporter so we print projects as they are loaded.
            Project project = await workspace.OpenProjectAsync(xgraphicsProjectPath, new ConsoleProgressReporter());
            Console.WriteLine($"Finished loading project '{xgraphicsProjectPath}'");

            GenerateClasses(workspace, project);
        }

        private static void GenerateClasses(Workspace workspace, Project project)
        {
            var compilation = project.GetCompilationAsync().Result;

            foreach (var tree in compilation.SyntaxTrees)
            {
                var interfaces = tree.GetRoot().DescendantNodesAndSelf().Where(x => x.IsKind(SyntaxKind.InterfaceDeclaration));
                foreach (SyntaxNode node in interfaces)
                {
                    InterfaceDeclarationSyntax interfaceDeclaration = (InterfaceDeclarationSyntax) node;

                    if (!HasModelObjectAttribute(interfaceDeclaration))
                        continue;

                    string rootDirectory = "c:\\myhome\\Dropbox\\faml\\XGraphics\\src";
                    try
                    {
                        Console.WriteLine($"Processing {interfaceDeclaration.Identifier.Text}");
                        new CompilationUnitGenerator(workspace, interfaceDeclaration, rootDirectory, WpfXamlOutputType.Instance).Generate();
                        new CompilationUnitGenerator(workspace, interfaceDeclaration, rootDirectory, XamarinFormsXamlOutputType.Instance).Generate();
                        new CompilationUnitGenerator(workspace, interfaceDeclaration, rootDirectory, StandardModelOutputType.Instance).Generate();
                    }
                    catch (UserViewableException e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                        Environment.Exit(1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.ToString()}");
                        Environment.Exit(2);
                    }
                }
            }
        }

        private static bool HasModelObjectAttribute(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            foreach (AttributeListSyntax attributeList in interfaceDeclaration.AttributeLists)
            {
                foreach (AttributeSyntax attribute in attributeList.Attributes)
                {
                    if (attribute.Name.ToString() == "GraphicsModelObject")
                        return true;
                }
            }

            return false;
        }

        private static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {
            Console.WriteLine("Multiple installs of MSBuild detected please select one:");
            for (int i = 0; i < visualStudioInstances.Length; i++)
            {
                Console.WriteLine($"Instance {i + 1}");
                Console.WriteLine($"    Name: {visualStudioInstances[i].Name}");
                Console.WriteLine($"    Version: {visualStudioInstances[i].Version}");
                Console.WriteLine($"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
            }

            while (true)
            {
                var userResponse = Console.ReadLine();
                if (int.TryParse(userResponse, out int instanceNumber) &&
                    instanceNumber > 0 &&
                    instanceNumber <= visualStudioInstances.Length)
                {
                    return visualStudioInstances[instanceNumber - 1];
                }
                Console.WriteLine("Input not accepted, try again.");
            }
        }

        private class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
        {
            public void Report(ProjectLoadProgress loadProgress)
            {
                var projectDisplay = Path.GetFileName(loadProgress.FilePath);
                if (loadProgress.TargetFramework != null)
                {
                    projectDisplay += $" ({loadProgress.TargetFramework})";
                }

                Console.WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
            }
        }
    }
}
