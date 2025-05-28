using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Rename;

var workspace = MSBuildWorkspace.Create();
var solution = await workspace.OpenSolutionAsync(
    "/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/RoslynMoreThanJustACompiler.sln");

foreach (var project in solution.Projects)
{
    var compilation = await project.GetCompilationAsync();
    var factAttributeSymbol = compilation.GetTypeByMetadataName("Xunit.FactAttribute")!;

    foreach (var document in project.Documents)
    {   
        var documentEditor = await DocumentEditor.CreateAsync(document);
        var methodDeclarationSyntaxes = documentEditor.OriginalRoot
            .DescendantNodes()
            .OfType<MethodDeclarationSyntax>();

        foreach (var methodDeclarationSyntax in methodDeclarationSyntaxes)
        {
            var methodSymbol = ModelExtensions.GetDeclaredSymbol(documentEditor.SemanticModel, methodDeclarationSyntax) as IMethodSymbol;
            var methodHasFactAttribute = methodSymbol.GetAttributes().Any(attributeData =>
                attributeData.AttributeClass.Equals(factAttributeSymbol, SymbolEqualityComparer.Default));
            
            if (methodHasFactAttribute)
            {
                solution = await Renamer.RenameSymbolAsync(solution, methodSymbol, new SymbolRenameOptions(), methodSymbol.Name.Pascalize());
            }
        }
    }
}

workspace.TryApplyChanges(solution);
