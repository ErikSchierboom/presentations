using Xunit;

namespace Solutions;

public class LeapTests
{
    [Fact]
    public void Year_not_divisible_by_4_in_common_year()
    {
        Assert.False(Leap.IsLeapYear(2015));
    }

    [Fact]
    public void Year_divisible_by_4_not_divisible_by_100_in_leap_year()
    {
        Assert.True(Leap.IsLeapYear(1996));
    }
}