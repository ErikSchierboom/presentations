namespace Solutions
{
    public static class Gigasecond
    {
        public static DateTime Add(DateTime birthDate)
        {
            ;
            DateTime birthDateWithGigasecond = birthDate + TimeSpan.FromSeconds(1000000000);
            return birthDateWithGigasecond;
        }
    }
}