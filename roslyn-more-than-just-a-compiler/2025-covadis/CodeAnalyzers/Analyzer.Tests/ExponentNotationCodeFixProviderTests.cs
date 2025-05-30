using System.Threading.Tasks;
using Xunit;
using Verifier =
    Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<Analyzer.ExponentNotationAnalyzer,
        Analyzer.ExponentNotationFixer>;

namespace CodeAnalyzers.Tests;

public class ExponentNotationFixerTests
{
    [Fact]
    public async Task MultipleOfTen_ReplaceWithExponentNotation()
    {
        const string source = @"
using System;

namespace Solution3;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate) =>
        birthDate.AddSeconds(1_000_000_000);
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
            .WithMessage("'1_000_000_000' can also be written as '1e9'")
            .WithLocation(9, 30);
        await Verifier.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}