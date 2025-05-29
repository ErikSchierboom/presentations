using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

const string sourceFilePath = @"/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/Solutions/TwoFer.cs";
var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFilePath));

var root = await syntaxTree.GetRootAsync();

var usesNamespaceDeclaration = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().Any();
if (usesNamespaceDeclaration)
{
    Console.WriteLine("Please use a file-scoped namespace declaration.");
    return; 
}

var classDeclaration = root.DescendantNodes()
    .OfType<ClassDeclarationSyntax>()
    .Single(classDeclaration => classDeclaration.Identifier.Text == "TwoFer");
var classIsNotStatic = !classDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword);
if (classIsNotStatic)
{
    Console.WriteLine("Please make the class static.");
    return; 
}

var usesMethodOverloading = classDeclaration
    .DescendantNodes()
    .OfType<MethodDeclarationSyntax>()
    .Count(methodDeclaration => methodDeclaration.Identifier.Text == "Greeting") > 1;
if (usesMethodOverloading)
{
    Console.WriteLine("Please use default parameters instead of method overloading.");
    return;
}

var methodDeclaration = classDeclaration
    .DescendantNodes()
    .OfType<MethodDeclarationSyntax>()
    .Single(methodDeclaration => methodDeclaration.Identifier.Text == "Greeting");
var useNullAsDefaultValue = methodDeclaration.ParameterList.Parameters is [{ Default.Value: not null }] &&
                                 methodDeclaration.ParameterList.Parameters[0].Default!.Value.IsKind(SyntaxKind.NullLiteralExpression);
if (useNullAsDefaultValue)
{
    Console.WriteLine("Please use a string as the default value.");
    return;
}

var usesStringConcatenation = methodDeclaration.DescendantNodes()
    .OfType<BinaryExpressionSyntax>()
    .Any(binaryExpression => binaryExpression.IsKind(SyntaxKind.AddExpression) &&
                             (binaryExpression.Left.IsKind(SyntaxKind.StringLiteralExpression) ||
                              binaryExpression.Right.IsKind(SyntaxKind.StringLiteralExpression)));
if (usesStringConcatenation)
{
    Console.WriteLine("Please use string interpolation instead of string concatenation.");
    return;
}

var usesBlockWithSingleStatement = methodDeclaration.Body?.Statements is [_];
if (usesBlockWithSingleStatement)
{
    Console.WriteLine("Please use an expression-bodied method instead of a block method.");
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
