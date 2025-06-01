using Microsoft.CodeAnalysis.CSharp;

var sourceCodeFilePath = Path.GetFullPath("../../../../Rewriting.Example/Gigasecond.cs");
var sourceCode = File.ReadAllText(sourceCodeFilePath);

var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
var root = await syntaxTree.GetRootAsync();

// TODO: format code
// TODO: remove empty statement
// TODO: use exponent notation

Console.WriteLine("Done");
