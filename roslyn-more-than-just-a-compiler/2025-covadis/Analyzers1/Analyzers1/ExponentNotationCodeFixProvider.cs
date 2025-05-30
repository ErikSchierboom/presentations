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
using Microsoft.CodeAnalysis.Rename;

namespace Analyzers1;

/// <summary>
/// A sample code fix provider that renames classes with the company name in their definition.
/// All code fixes must  be linked to specific analyzers.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ExponentNotationCodeFixProvider)), Shared]
public class ExponentNotationCodeFixProvider : CodeFixProvider
{
    private const string CommonName = "Common";

    // Specify the diagnostic IDs of analyzers that are expected to be linked.
    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(ExponentNotationAnalyzer.DiagnosticId);

    // If you don't need the 'fix all' behaviour, return null.
    public override FixAllProvider? GetFixAllProvider() => null;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.Single();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        var diagnosticNode = root?.FindNode(diagnosticSpan, getInnermostNodeForTie: true);

        if (diagnosticNode is not LiteralExpressionSyntax literalExpression)
            return;

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                title: string.Format(Resources.ES0001CodeFixTitle, literalExpression.Token.Text, "1e9"),
                createChangedSolution: c => SanitizeCompanyNameAsync(context.Document, literalExpression, c),
                equivalenceKey: nameof(Resources.ES0001CodeFixTitle)),
            diagnostic);
    }

    /// <summary>
    /// Executed on the quick fix action raised by the user.
    /// </summary>
    /// <param name="document">Affected source file.</param>
    /// <param name="literalExpression">Highlighted class declaration Syntax Node.</param>
    /// <param name="cancellationToken">Any fix is cancellable by the user, so we should support the cancellation token.</param>
    /// <returns>Clone of the solution with updates: renamed class.</returns>
    private async Task<Solution> SanitizeCompanyNameAsync(Document document,
        LiteralExpressionSyntax literalExpression, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken);
        if (root is null)
            return document.Project.Solution;
        
        var newToken = SyntaxFactory.Literal("1e9", 1e9).WithTriviaFrom(literalExpression.Token);
        var newLiteralExpression = literalExpression.WithToken(newToken);

        var newRoot = root.ReplaceNode(literalExpression, newLiteralExpression);
        var newDocument = document.WithSyntaxRoot(newRoot);

        return newDocument.Project.Solution;
    }
}