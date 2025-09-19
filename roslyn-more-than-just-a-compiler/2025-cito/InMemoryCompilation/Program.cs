using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

const string movieSource = @"
record Movie(string Title, int Year);
";
var syntaxTree = CSharpSyntaxTree.ParseText(movieSource);
var compilation = CSharpCompilation.Create("InMemoryCompilation",
        syntaxTrees: [syntaxTree],
        references: [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

// TODO: emit IL code
// TODO: load assembly from IL code
// TODO: create instance of Movie

Console.WriteLine("Done");
