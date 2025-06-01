// Roslyn Quoter: https://roslynquoter.azurewebsites.net/

string[] recordNames = ["Movie", "Director", "Actor"];

foreach (var recordName in recordNames)
{
    var recordFilePath = Path.GetFullPath($"../../../{recordName}.cs");
    
    // TODO: generate source code
    // TODO: write source code to file
}

Console.WriteLine("TODO: generate source code");
