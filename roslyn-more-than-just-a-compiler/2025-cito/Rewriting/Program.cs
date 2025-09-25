using Microsoft.CodeAnalysis.CSharp;

var sourceCodeFilePath = Path.GetFullPath("../../../../Rewriting.Example/Gigasecond.cs");
var sourceCode = File.ReadAllText(sourceCodeFilePath);

var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
var root = syntaxTree.GetRoot();

// TODO: normalize whitespace
// TODO: remove empty statements - via method
// TODO: remove empty statements - via rewriter
// TODO: use exponent notation

Console.WriteLine("Done");
