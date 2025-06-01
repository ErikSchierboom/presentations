using Microsoft.CodeAnalysis.CSharp;

var sourceCodeFilePath = Path.GetFullPath("../../../../Rewriting.Example/Gigasecond.cs");
var sourceCode = File.ReadAllText(sourceCodeFilePath);

var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
var root = await syntaxTree.GetRootAsync();

Console.WriteLine("Rewritten");

// TODO: remove empty statements
// TODO: use exponent notation
// TODO: use expression-bodied members
