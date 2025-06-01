using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

var sourceFilePath = Path.GetFullPath("../../../../Analyzing.Example/Gigasecond.cs");
var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFilePath));
var root = await syntaxTree.GetRootAsync();

var doesNotUseFileScopedNamespace = !root.DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().Any();
if (doesNotUseFileScopedNamespace)
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
var canUseExponentNotation = methodDeclaration
    .DescendantTokens()
    .Any(token => token.IsKind(SyntaxKind.NumericLiteralToken) && 
                  token.Value is 1e9 &&
                  token.Text != "1e9");
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

var dateTimeStruct = compilation.GetSpecialType(SpecialType.System_DateTime);
var addSecondsCall = dateTimeStruct.GetMembers("AddSeconds").Single();
var invocationExpression = root.DescendantNodes()
    .OfType<InvocationExpressionSyntax>()
    .Single();
var operation = (IInvocationOperation)semanticModel.GetOperation(invocationExpression)!;
var doesNotUseAddSeconds = !operation.TargetMethod.Equals(addSecondsCall, SymbolEqualityComparer.Default);
if (doesNotUseAddSeconds)
{
    Console.WriteLine("Please use the DateTime.AddSeconds method.");
    return;
}
