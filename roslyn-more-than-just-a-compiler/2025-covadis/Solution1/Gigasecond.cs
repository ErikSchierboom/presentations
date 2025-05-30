using System;

namespace Solution1
{
    public static class Gigasecond
    {
        public static DateTime Add(DateTime birthDate)
        {
            return birthDate + TimeSpan.FromSeconds(1000000000);
        }
    }
}