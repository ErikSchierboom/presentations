using System;
using Xunit;

namespace Analyzer.Example;

public class GigasecondTests
{
    [Fact]
    public void Date_only_specification_of_time()
    {
        Assert.Equal(
            new DateTime(2043, 1, 1, 1, 46, 40),
            Gigasecond.Add(new DateTime(2011, 4, 25)));
    }

    [Fact]
    public void Second_test_for_date_only_specification_of_time()
    {
        Assert.Equal(
            new DateTime(2009, 2, 19, 1, 46, 40),
            Gigasecond.Add(new DateTime(1977, 6, 13)));
    }
}