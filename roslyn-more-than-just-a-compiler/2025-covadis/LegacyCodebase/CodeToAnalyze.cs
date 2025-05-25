using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DefaultNamespace
{
    class CodeToAnalyze
    {
        public void Hi()
        {
            string message = "Hello" + " World!";
            Debug.WriteLine(message);
        }

        public async void Sleepy()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}
