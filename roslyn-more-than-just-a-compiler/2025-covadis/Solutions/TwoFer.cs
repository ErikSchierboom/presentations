using System.Diagnostics;

namespace Solutions
{
    public static class TwoFer
    {
        public static string Greeting(string name = null)
        {
            if (name == null)
            {
                return "Hello you!";
            }
            
            Debug.WriteLine("Greeting with name");
            return "Hello " + name + "!";
        }
    }
}
