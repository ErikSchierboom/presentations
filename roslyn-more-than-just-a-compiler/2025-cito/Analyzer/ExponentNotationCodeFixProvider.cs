using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Analyzer;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ExponentNotationCodeFixProvider)), Shared]
public class ExponentNotationCodeFixProvider : CodeFixProvider
{
    private const string Title = "Convert to exponent notation";
    
    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(ExponentNotationAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        // TODO: create code action
        // TODO: register code fix

        return Task.CompletedTask;
    }

    private static Task<Solution> UseExponentNotation(Document document, TextSpan span, CancellationToken ct)
    {
        throw new NotImplementedException();
        
        // TODO: show debugging test
    }
}
