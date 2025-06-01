using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;

namespace Analyzer;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddSecondsCodeFixProvider)), Shared]
public class AddSecondsCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
        [AddSecondsAnalyzer.DiagnosticId];

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        // TODO: find the invocation operation
        // TODO: register code fix

        // context.RegisterCodeFix(
        //     CodeAction.Create(
        //         title: string.Format(Resources.ES0002CodeFixTitle, memberAccessExpression.Name.Identifier.Text),
        //         createChangedSolution: cancellationToken => ConvertToAddSeconds(context.Document, invocationExpression, memberAccessExpression, cancellationToken),
        //         equivalenceKey: nameof(Resources.ES0002CodeFixTitle)),
        //     diagnostic);
    }
}