using System;
using System.Threading.Tasks;

namespace DefaultNamespace;

public class CodeToAnalyze
{
    public void Hi()
    {
        string message = "Hello" + " World!";
        Console.WriteLine(message);
    }

    public async void Sleepy()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
    }
}
