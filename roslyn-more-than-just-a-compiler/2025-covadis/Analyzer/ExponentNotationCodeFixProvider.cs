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

namespace Analyzer;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ExponentNotationCodeFixProvider)), Shared]
public class ExponentNotationCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(ExponentNotationAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.Single();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        var diagnosticNode = root?.FindNode(diagnosticSpan, getInnermostNodeForTie: true);

        if (diagnosticNode is not LiteralExpressionSyntax literalExpression)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                title: string.Format(Resources.ES0001CodeFixTitle, literalExpression.Token.Text),
                createChangedSolution: cancellationToken => ConvertToExponentNotation(context.Document, literalExpression, cancellationToken),
                equivalenceKey: nameof(Resources.ES0001CodeFixTitle)),
            diagnostic);
    }

    private static async Task<Solution> ConvertToExponentNotation(Document document,
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