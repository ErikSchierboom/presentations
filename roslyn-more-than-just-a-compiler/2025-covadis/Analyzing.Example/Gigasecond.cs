using System;

namespace Analyzing.Example;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        return birthDate.AddSeconds(1e9);
    }
}