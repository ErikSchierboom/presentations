using Microsoft.CodeAnalysis.CSharp;

var sourceFilePath = Path.GetFullPath("../../../../Rewriting.Example/Gigasecond.cs");

var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFilePath));
var root = await syntaxTree.GetRootAsync();

Console.WriteLine("Rewritten");

// TODO: remove empty statements
// TODO: use exponent notation
// TODO: use expression-bodied members
