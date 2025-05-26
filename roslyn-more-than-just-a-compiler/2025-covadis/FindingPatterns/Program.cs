using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Operations;

var syntaxTree = CSharpSyntaxTree.ParseText(
    File.ReadAllText(
        @"/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/Solutions/TwoFer.cs"));

var root = await syntaxTree.GetRootAsync();

var usesNamespaceDeclaration = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().Any();
if (usesNamespaceDeclaration)
{
    Console.WriteLine("Please use a file-scoped namespace declaration.");
    return; 
}

var twoFerClassDeclaration = root.DescendantNodes()
    .OfType<ClassDeclarationSyntax>()
    .Single(classDeclaration => classDeclaration.Identifier.ValueText == "TwoFer");

var classIsNotPublic = !twoFerClassDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword);
if (classIsNotPublic)
{
    Console.WriteLine("Please make the class public.");
    return; 
}

var usesMethodOverloading = twoFerClassDeclaration
    .DescendantNodes()
    .OfType<MethodDeclarationSyntax>()
    .Count(methodDeclaration => methodDeclaration.Identifier.Text == "Greeting") > 1;

if (usesMethodOverloading)
{
    Console.WriteLine("Please use default parameters instead of method overloading.");
    return;
}

var greetingMethodDeclaration = twoFerClassDeclaration
    .DescendantNodes()
    .OfType<MethodDeclarationSyntax>()
    .Single(methodDeclaration => methodDeclaration.Identifier.Text == "Greeting");

var useNullAsDefaultValue = greetingMethodDeclaration.ParameterList.Parameters is [{ Default.Value: not null }] &&
                                 greetingMethodDeclaration.ParameterList.Parameters[0].Default!.Value.IsKind(SyntaxKind.NullLiteralExpression);

if (useNullAsDefaultValue)
{
    Console.WriteLine("Please use a string as the default value.");
    return;
}

var usesStringConcatenation = greetingMethodDeclaration.DescendantNodes()
    .OfType<BinaryExpressionSyntax>()
    .Any(binaryExpression => binaryExpression.IsKind(SyntaxKind.AddExpression) &&
                             (binaryExpression.Left.IsKind(SyntaxKind.StringLiteralExpression) ||
                              binaryExpression.Right.IsKind(SyntaxKind.StringLiteralExpression)));
if (usesStringConcatenation)
{
    Console.WriteLine("Please use string interpolation instead of string concatenation.");
    return;
}

if (greetingMethodDeclaration.Body?.Statements is [_])
{
    Console.WriteLine("Please use an expression-bodied method instead of a block method.");
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

var usesDebug = invocationOperations.Any(invocationOperation => invocationOperation.TargetMethod.ContainingType.Equals(typeByMetadataName, SymbolEqualityComparer.Default));
if (usesDebug)
{
    Console.WriteLine("Dont use the Debug class.");
    return;
}

// 1. Gebruikt overloading
// 2. Gebruikt `null` als default waarde
// 3. Gebruikt string concatenatie
// 4. Gebruik block method
// 5. Roept `Debug.WriteLine` aan
