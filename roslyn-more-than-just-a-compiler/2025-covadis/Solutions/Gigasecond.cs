using Dbg = System.Diagnostics.Debug;

namespace Solutions
{
    public static class Gigasecond
    {
        public static DateTime Add(DateTime birthDate)
        {
            Dbg.WriteLine("Gigasecond.Add called with birthDate: " + birthDate);
            ;
            DateTime birthDateWithGigasecond = birthDate + TimeSpan.FromSeconds(1000000000);
            return birthDateWithGigasecond;
        }
    }
}