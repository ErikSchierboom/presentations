using System;

namespace Analyzers1.Sample;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate) =>
        birthDate.AddSeconds(1_000_000_000);
}