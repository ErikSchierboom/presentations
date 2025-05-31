using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Xunit;
using Verifier =
    Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
        Analyzers.AddSecondsAnalyzer>;

namespace Analyzers.Tests;

public class AddSecondsAnalyzerTests
{   
    [Fact]
    public async Task UsesAddMilliseconds_AlertsDiagnostic()
    {
        const string source = @"
using System;

namespace Solution3;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate) =>
        birthDate.AddMilliseconds(1e9);
}
";

        var expected = Verifier.Diagnostic()
            .WithMessage("Use 'AddSeconds' instead of 'AddMilliseconds'")
            .WithSeverity(DiagnosticSeverity.Warning)
            .WithLocation(9, 9);
        await Verifier.VerifyAnalyzerAsync(source, expected);
    }
    [Fact]
    public async Task UsesAddSeconds_DoesNotAlertDiagnostic()
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