using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

var sourceCodeFilePath = Path.GetFullPath("../../../../Analyzing.Example/Gigasecond.cs");
var sourceCode = File.ReadAllText(sourceCodeFilePath);

var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
var root = syntaxTree.GetRoot();

var canUseFileScopedNamespace = !root.ChildNodes()
    .OfType<FileScopedNamespaceDeclarationSyntax>()
    .Any();
if (canUseFileScopedNamespace)
{
    Console.WriteLine("Please use a file-scoped namespace.");
    return;
}

var canUseExponentNotation = root.DescendantTokens()
    .Any(token => token.IsKind(SyntaxKind.NumericLiteralToken) &&
                  token.Value is 1_000_000_000 &&
                  token.Text != "1e9");
if (canUseExponentNotation)
{
    Console.WriteLine("Please use exponent notation.");
    return;
}

var compilation = CSharpCompilation.Create("Gigasecond",
    syntaxTrees: [syntaxTree],
    references: [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
var invocationExpression = root.DescendantNodes()
    .OfType<InvocationExpressionSyntax>()
    .First();
var semanticModel = compilation.GetSemanticModel(syntaxTree);
var invocationOperation = (IInvocationOperation)semanticModel.GetOperation(invocationExpression)!;
var dateTimeType = compilation.GetSpecialType(SpecialType.System_DateTime);
var addSecondsMethod = dateTimeType.GetMembers("AddSeconds").First();
if (!invocationOperation.TargetMethod.Equals(addSecondsMethod, SymbolEqualityComparer.Default))
{
    Console.WriteLine("Use AddSeconds.");
    return;
}

Console.WriteLine("Analyzed");
