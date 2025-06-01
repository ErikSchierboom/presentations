using Microsoft.CodeAnalysis.CSharp.Scripting;

Console.Write("> ");
var code = Console.ReadLine();

var state = await CSharpScript.RunAsync(code);
Console.WriteLine(state.ReturnValue);
foreach (var variable in state.Variables)
    Console.WriteLine($"{variable.Name} = {variable.Value}");

while (true)
{
    Console.Write("> ");
    code = Console.ReadLine();
    state = await state.ContinueWithAsync(code);
    Console.WriteLine(state.ReturnValue);
    foreach (var variable in state.Variables)
        Console.WriteLine($"{variable.Name} = {variable.Value}");
}
