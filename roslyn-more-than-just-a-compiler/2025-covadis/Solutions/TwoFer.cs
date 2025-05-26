using Dbg = System.Diagnostics.Debug;

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
            Dbg.WriteLine("Greeting called with name");
            return "Hello " + name + "!";
        }
    }
}