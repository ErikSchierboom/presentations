using Xunit;

namespace Solutions;

public class TwoFerTests
{
    [Fact]
    public void Greeting_WithoutName_UsesDefault()
    {
        Assert.Equal("Hello you", TwoFer.Greeting());
    }

    [Fact]
    public void Greeting_WithName_UsesName()
    {
        Assert.Equal("Hello Jane", TwoFer.Greeting("Jane"));
    }
}
