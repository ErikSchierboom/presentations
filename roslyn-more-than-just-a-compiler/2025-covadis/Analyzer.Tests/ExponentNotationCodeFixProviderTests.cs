using System.Threading.Tasks;
using Xunit;
using Verifier =
    Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<Analyzer.ExponentNotationAnalyzer,
        Analyzer.ExponentNotationCodeFixProvider>;

namespace Analyzer.Tests;

public class ExponentNotationCodeFixProviderTests
{
    [Fact]
    public async Task Gigasecond_ReplaceWithExponentNotation()
    {
        const string source = @"
using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate) =>
        birthDate.AddSeconds(1_000_000_000);
}
";

        const string fixedSource = @"
using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate) =>
        birthDate.AddSeconds(1e9);
}
";

        var expected = Verifier.Diagnostic()
            .WithMessage("Use exponent notation")
            .WithLocation(7, 30);
        await Verifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}