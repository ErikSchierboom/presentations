using System.Threading.Tasks;
using Xunit;
using Verifier =
    Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<Analyzer.AddSecondsAnalyzer,
        Analyzer.AddSecondsCodeFixProvider>;

namespace Analyzer.Tests;

public class AddSecondsCodeFixProviderTests
{
    [Fact]
    public async Task UsesAddMilliseconds_ReplaceWithAddSeconds()
    {
        const string source = @"
using System;

namespace Solution3;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate) =>
        birthDate.AddMilliseconds(1e12);
}
";

        const string fixedSource = @"
using System;

namespace Solution3;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate) =>
        birthDate.AddSeconds(1e9);
}
";

        var expected = Verifier.Diagnostic()
            .WithMessage("Use 'AddSeconds' instead of 'AddMilliseconds'")
            .WithLocation(9, 9);
        await Verifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}