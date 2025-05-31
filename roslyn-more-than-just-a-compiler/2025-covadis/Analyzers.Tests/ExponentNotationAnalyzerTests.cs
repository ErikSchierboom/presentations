using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Xunit;
using Verifier =
    Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
        Analyzers.ExponentNotationAnalyzer>;

namespace Analyzers.Tests;

public class ExponentNotationAnalyzerTests
{   
    [Fact]
    public async Task CanUseExponentNotation_AlertsDiagnostic()
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

        var expected = Verifier.Diagnostic()
            .WithMessage("Convert '1_000_000_000' to exponent notation")
            .WithSeverity(DiagnosticSeverity.Warning)
            .WithLocation(9, 30);
        await Verifier.VerifyAnalyzerAsync(source, expected);
    }
    [Fact]
    public async Task AlreadyUsesExponentNotation_DoesNotAlertDiagnostic()
    {
        const string source = @"
using System;

namespace Solution3;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate) =>
        birthDate.AddSeconds(1e9);
}
";

        await Verifier.VerifyAnalyzerAsync(source);
    }
}