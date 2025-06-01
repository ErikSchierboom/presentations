using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Rename;

var solutionFilePath = Path.GetFullPath("../../../../RoslynMoreThanJustACompiler.sln");
var workspace = MSBuildWorkspace.Create();
var solution = await workspace.OpenSolutionAsync(solutionFilePath);

foreach (var project in solution.Projects)
{
    var compilation = await project.GetCompilationAsync();
    var factAttribute = compilation!.GetTypeByMetadataName("Xunit.FactAttribute")!;

    foreach (var document in project.Documents)
    {
        var documentEditor = await DocumentEditor.CreateAsync(document);
        var methods = documentEditor.OriginalRoot
            .DescendantNodes()
            .OfType<MethodDeclarationSyntax>();

        foreach (var method in methods)
        {
            var methodSymbol = (IMethodSymbol)documentEditor.SemanticModel.GetDeclaredSymbol(method)!;
            var isTestMethod = methodSymbol.GetAttributes()
                .Any(attribute => attribute.AttributeClass!.Equals(factAttribute, SymbolEqualityComparer.Default));
            if (isTestMethod)
            {
                solution = await Renamer.RenameSymbolAsync(solution, methodSymbol, new SymbolRenameOptions(),
                    methodSymbol.Name.Pascalize());
            }
        }
    }
}

workspace.TryApplyChanges(solution);

Console.WriteLine("Renamed");

// TODO: load workspace
// TODO: enumerate documents
// TODO: rename tests
