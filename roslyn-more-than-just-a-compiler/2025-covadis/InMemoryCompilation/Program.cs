using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

const string greeterClassSource = @"
public class Greeter
{
    public string Greet(string name)
    {
        return $""Hello, {name}!"";
    }
}
";

var syntaxTree = CSharpSyntaxTree.ParseText(greeterClassSource);

const string assemblyName = "GreeterAssembly";
var compilation = CSharpCompilation.Create(
    assemblyName,
    syntaxTrees: [syntaxTree],
    references: [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

var stream = new MemoryStream();
var emitResult = compilation.Emit(stream);
if (!emitResult.Success)
{
    foreach (var diagnostic in emitResult.Diagnostics)
        Console.WriteLine(diagnostic);

    return;
}

var assembly = Assembly.Load(stream.ToArray());
var greeterClass = assembly.GetType("Greeter")!;
var greeterInstance = Activator.CreateInstance(greeterClass);
var greeting = greeterClass.GetMethod("Greet")!.Invoke(greeterInstance, ["World"]);

Console.WriteLine(greeting);
