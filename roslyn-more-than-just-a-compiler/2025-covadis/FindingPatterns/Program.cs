using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Operations;

var syntaxTree = CSharpSyntaxTree.ParseText(
    File.ReadAllText(
        @"/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/Solutions/TwoFer.cs"));

var root = await syntaxTree.GetRootAsync();

var usesMethodOverloading = root
    .DescendantNodes()
    .OfType<MethodDeclarationSyntax>()
    .Count(methodDeclaration => methodDeclaration.Identifier.Text == "Greeting") > 1;

if (usesMethodOverloading)
{
    Console.WriteLine("Please use default parameters instead of method overloading.");
    return;
}

var workspace = MSBuildWorkspace.Create();
var project = await workspace.OpenProjectAsync(
    "/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/Solutions/Solutions.csproj");

var document = project.Documents.First(document => document.Name == "TwoFer.cs");
var semanticModel = await document.GetSemanticModelAsync();
var documentRoot = await document.GetSyntaxRootAsync();

var invocationOperations = documentRoot!
    .DescendantNodes()
    .OfType<InvocationExpressionSyntax>()
    .Select(invocationExpression => semanticModel!.GetOperation(invocationExpression))
    .OfType<IInvocationOperation>()
    .ToArray();

var typeByMetadataName = semanticModel.Compilation.GetTypeByMetadataName("System.Diagnostics.Debug");

var any = invocationOperations.Any(invocationOperation => invocationOperation.TargetMethod.ContainingType.Equals(typeByMetadataName, SymbolEqualityComparer.Default));
Console.WriteLine();
// 1. Gebruikt overloading
// 2. Gebruikt `null` als default waarde
// 3. Gebruikt geen string interpolatie
// 4. Gebruik block method
// 5. Roept `Debug.WriteLine` aan
