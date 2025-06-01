using Microsoft.CodeAnalysis.CSharp.Scripting;

Console.Write("> ");
var sourceCode = Console.ReadLine();
var state = await CSharpScript.RunAsync(sourceCode);
Console.WriteLine($"Return value: {state.ReturnValue}");

while (true)
{
    Console.Write("> ");
    sourceCode = Console.ReadLine();
    state = await state.ContinueWithAsync(sourceCode);
    Console.WriteLine($"Return value: {state.ReturnValue}");
}
