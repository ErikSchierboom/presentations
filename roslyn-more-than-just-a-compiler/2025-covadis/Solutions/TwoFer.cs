using System.Diagnostics;

namespace Solutions
{
    public static class TwoFer
    {
        public static string Greeting()
        {
            return "Hello you!";
        }

        public static string Greeting(string name)
        {
            Debug.WriteLine("Greeting with name");
            return "Hello " + name + "!";
        }
    }
}
