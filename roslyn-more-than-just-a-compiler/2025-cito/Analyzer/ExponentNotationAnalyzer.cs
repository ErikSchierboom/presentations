using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ExponentNotationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "ES0001";
    private const string Category = "Naming";
    private const string Title = "Use exponent notation";
    private const string MessageFormat = "Use exponent notation";
    private const string Description = "Use exponent notation to simplify multiples of ten.";

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = 
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        
        context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.NumericLiteralExpression);
    }

    private static void Analyze(SyntaxNodeAnalysisContext context)
    {
        var expression = (LiteralExpressionSyntax)context.Node;
        var token = expression.Token;

        if (token.IsKind(SyntaxKind.NumericLiteralToken) &&
            token.Value is 1_000_000_000 &&
            token.Text != "1e9")
        {
            var diagnostic = Diagnostic.Create(Rule, expression.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }
}