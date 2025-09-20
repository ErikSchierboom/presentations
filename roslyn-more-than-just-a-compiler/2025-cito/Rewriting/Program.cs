using Microsoft.CodeAnalysis.CSharp;

var sourceCodeFilePath = Path.GetFullPath("../../../../Rewriting.Example/Gigasecond.cs");
var sourceCode = File.ReadAllText(sourceCodeFilePath);

var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
var root = syntaxTree.GetRoot();

// TODO: normalize whitespace
// TODO: remove empty statements
// TODO: use exponent notation

Console.WriteLine("Done");
