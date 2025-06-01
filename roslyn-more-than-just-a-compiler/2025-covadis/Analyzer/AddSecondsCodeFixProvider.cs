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

namespace Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddSecondsCodeFixProvider)), Shared]
public class AddSecondsCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(AddSecondsAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.Single();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        var diagnosticNode = root?.FindNode(diagnosticSpan, getInnermostNodeForTie: true);

        if (diagnosticNode is not InvocationExpressionSyntax invocationExpression)
            return;

        if (invocationExpression.Expression is not MemberAccessExpressionSyntax memberAccessExpression)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                title: string.Format(Resources.ES0002CodeFixTitle, memberAccessExpression.Name.Identifier.Text),
                createChangedSolution: cancellationToken => ConvertToAddSeconds(context.Document, invocationExpression, memberAccessExpression, cancellationToken),
                equivalenceKey: nameof(Resources.ES0002CodeFixTitle)),
            diagnostic);
    }

    private static async Task<Solution> ConvertToAddSeconds(Document document,
        InvocationExpressionSyntax invocationExpression, 
        MemberAccessExpressionSyntax memberAccessExpression,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken);
        if (root is null)
            return document.Project.Solution;

        var newMemberAccessExpression = memberAccessExpression.WithName(
            SyntaxFactory.IdentifierName("AddSeconds"));
        
        var newInvocationExpression = invocationExpression
            .WithExpression(newMemberAccessExpression)
            .WithArgumentList(SyntaxFactory.ArgumentList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Argument(
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression, 
                            SyntaxFactory.Literal("1e9", 1e9))))));

        var newRoot = root.ReplaceNode(invocationExpression, newInvocationExpression);
        var newDocument = document.WithSyntaxRoot(newRoot);
        return newDocument.Project.Solution;
    }
}