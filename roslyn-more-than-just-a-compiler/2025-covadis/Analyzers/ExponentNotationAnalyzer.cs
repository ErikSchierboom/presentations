using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ExponentNotationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "ES0001";

    private const string Title = "Convert to exponent notation";
    private const string MessageFormat = "Convert '{0}' to exponent notation";
    private const string Description = "Multiples of ten can be written in exponent notation.";
    private const string Category = "Naming";

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.NumericLiteralExpression);
    }

    private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not LiteralExpressionSyntax literalExpression ||
            !literalExpression.Token.IsKind(SyntaxKind.NumericLiteralToken) ||
            literalExpression.Token.Value is not 1000000000 ||
            literalExpression.Token.Text == "1e9")
            return;

        var diagnostic = Diagnostic.Create(Rule,
            literalExpression.GetLocation(),
            literalExpression.Token.Text);
        context.ReportDiagnostic(diagnostic);
    }
}