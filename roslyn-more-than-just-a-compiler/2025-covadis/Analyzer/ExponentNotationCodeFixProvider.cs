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
        [ExponentNotationAnalyzer.DiagnosticId];

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.Single();
        var codeAction = CodeAction.Create(
            Title,
            createChangedSolution: ct => UseExponentNotation(context.Document, diagnostic.Location.SourceSpan, ct),
            Title);
        context.RegisterCodeFix(codeAction, diagnostic);
    }

    private static async Task<Solution> UseExponentNotation(Document document, TextSpan textSpan, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken);
        var literalExpression = (LiteralExpressionSyntax)root.FindNode(textSpan, getInnermostNodeForTie: true);
        
        var newLiteralExpression = literalExpression.WithToken(SyntaxFactory.Literal("1e9", 1e9));
        var newRoot = root.ReplaceNode(literalExpression, newLiteralExpression);
        var newDocument = document.WithSyntaxRoot(newRoot);
        return newDocument.Project.Solution;
    }
}
