// Roslyn Quoter: https://roslynquoter.azurewebsites.net/

string[] recordNames = ["Movie", "Director", "Actor"];

foreach (var recordName in recordNames)
{
    var recordFilePath = Path.GetFullPath($"../../../{recordName}.cs");

    // TODO: create record
    // TODO: add modifiers
    // TODO: add auto-generated comment
    // TODO: write code to file
}

Console.WriteLine("Done");
