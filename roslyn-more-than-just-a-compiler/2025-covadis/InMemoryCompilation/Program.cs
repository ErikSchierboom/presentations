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

using var ms = new MemoryStream();
compilation.Emit(ms);

var assembly = Assembly.Load(ms.ToArray());
var movieType = assembly.GetType("Movie");
var movie = Activator.CreateInstance(movieType, "Inception", 2010);
Console.WriteLine(movie);
Console.WriteLine("Emitted");

// TODO: emit IL code to stream
// TODO: load assembly
// TODO: create instance of Movie
