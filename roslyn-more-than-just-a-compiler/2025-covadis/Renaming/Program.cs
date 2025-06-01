using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Rename;

var solutionFilePath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "RoslynMoreThanJustACompiler.sln"));
using var workspace = MSBuildWorkspace.Create();
var solution = await workspace.OpenSolutionAsync(solutionFilePath);

foreach (var project in solution.Projects)
{
    var compilation = await project.GetCompilationAsync() ?? throw new InvalidOperationException("Could not compile project");
    var factAttributeSymbol = compilation.GetTypeByMetadataName("Xunit.FactAttribute")!;

    foreach (var document in project.Documents)
    {   
        var documentEditor = await DocumentEditor.CreateAsync(document);
        var methodDeclarationSyntaxes = documentEditor.OriginalRoot
            .DescendantNodes()
            .OfType<MethodDeclarationSyntax>();

        foreach (var methodDeclarationSyntax in methodDeclarationSyntaxes)
        {
            var methodSymbol = documentEditor.SemanticModel.GetDeclaredSymbol(methodDeclarationSyntax) ?? throw new InvalidOperationException("Could not get symbol");
            var methodHasFactAttribute = methodSymbol.GetAttributes().Any(attributeData =>
                attributeData.AttributeClass is not null &&
                attributeData.AttributeClass.Equals(factAttributeSymbol, SymbolEqualityComparer.Default));

            if (methodHasFactAttribute)
                solution = await Renamer.RenameSymbolAsync(solution, methodSymbol, new SymbolRenameOptions(), methodSymbol.Name.Pascalize());
        }
    }
}

workspace.TryApplyChanges(solution);
