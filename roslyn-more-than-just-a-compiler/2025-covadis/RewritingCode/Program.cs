using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;

var workspace = MSBuildWorkspace.Create();
var project = await workspace.OpenProjectAsync(
    "/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/Solutions/Solutions.csproj");

var document = project.Documents.First(document => document.Name == "Leap.cs");
var semanticModel = await document.GetSemanticModelAsync();
var documentRoot = await document.GetSyntaxRootAsync();

var symbol = documentRoot.DescendantNodes()
    .OfType<VariableDeclaratorSyntax>()
    .Select(variableDeclarator => semanticModel.GetDeclaredSymbol(variableDeclarator))
    .FirstOrDefault();

var findReferencesAsync = await SymbolFinder.FindReferencesAsync(symbol, project.Solution);

Console.WriteLine();