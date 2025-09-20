using System;

namespace Rewriting.Example
{
    public static class Gigasecond
    {
        public static DateTime Add(DateTime birthDate)
        {
            Log_Date_Time(birthDate);
            return birthDate.AddSeconds(1e9);
        }

        private static void Log_Date_Time(DateTime dateTime)
        {
            Console.WriteLine(dateTime);
        }
    }
}