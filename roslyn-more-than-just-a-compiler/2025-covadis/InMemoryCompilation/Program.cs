using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

const string movieSource = @"
record Movie(string Title, int Year);
";

var syntaxTree = CSharpSyntaxTree.ParseText(movieSource);

const string assemblyName = "MovieAssembly";
var compilation = CSharpCompilation.Create(
    assemblyName,
    syntaxTrees: [syntaxTree],
    references: [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

var stream = new MemoryStream();
compilation.Emit(stream);

var assembly = Assembly.Load(stream.ToArray());
var movieType = assembly.GetType("Movie")!;
var movieInstance = Activator.CreateInstance(movieType, "Inception", 2010)!;

Console.WriteLine(movieInstance);
