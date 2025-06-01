// TODO: run code from command-line
// TODO: run code from command-line using previous state

using Microsoft.CodeAnalysis.CSharp.Scripting;

Console.Write("> ");
var sourceCode = Console.ReadLine();
var state = await CSharpScript.RunAsync(sourceCode);
Console.WriteLine(state.ReturnValue);

while (true)
{
    Console.Write("> ");
    sourceCode = Console.ReadLine();
    state = await state.ContinueWithAsync(sourceCode);
    Console.WriteLine(state.ReturnValue);
}

Console.WriteLine("Done");
