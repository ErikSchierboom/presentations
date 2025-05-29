using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

const string sourceFilePath = @"/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/Solutions/Gigasecond.cs";
var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFilePath));

var root = await syntaxTree.GetRootAsync();

var canUseFileScopedNamespace = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().Any();
if (canUseFileScopedNamespace)
{
    Console.WriteLine("Please use a file-scoped namespace declaration.");
    return; 
}

var classDeclaration = root.DescendantNodes()
    .OfType<ClassDeclarationSyntax>()
    .Single(classDeclaration => classDeclaration.Identifier.Text == "Gigasecond");
var classIsNotStatic = !classDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword);
if (classIsNotStatic)
{
    Console.WriteLine("Please make the class static.");
    return; 
}

var methodDeclaration = classDeclaration.Members
    .OfType<MethodDeclarationSyntax>()
    .Single(methodDeclaration => methodDeclaration.Identifier.Text == "Add");
var canUseExpressionBody = methodDeclaration.Body?.Statements is [_];
if (canUseExpressionBody)
{
    Console.WriteLine("Please use an expression-bodied method instead of a block method.");
    return;
}

var canUseExponentNotation = methodDeclaration
    .DescendantTokens()
    .Any(token => token.IsKind(SyntaxKind.NumericLiteralToken) && 
                  token is { Value: 1000000000, Text: "1000000000" });
if (canUseExponentNotation)
{
    Console.WriteLine("Please use exponent notation for the number 1,000,000,000.");
    return;
}

var compilation = CSharpCompilation.Create(
    "Analyzing",
    syntaxTrees: [syntaxTree],
    references: [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
var diagnostics = compilation.GetDiagnostics();
var semanticModel = compilation.GetSemanticModel(syntaxTree);

var debugClass = compilation.GetTypeByMetadataName("System.Diagnostics.Debug") ??
                         throw new InvalidOperationException("Could not find System.Diagnostics.Debug type");

var invocationExpression = root.DescendantNodes()
    .OfType<InvocationExpressionSyntax>()
    .Single();
var operation = (IInvocationOperation)(semanticModel.GetOperation(invocationExpression) ?? throw new InvalidOperationException("Could not get operation"));
if (operation.TargetMethod.ContainingType.Equals(debugClass, SymbolEqualityComparer.Default))
{
    Console.WriteLine("Please do not use the System.Diagnostics.Debug class.");
    return;
}
