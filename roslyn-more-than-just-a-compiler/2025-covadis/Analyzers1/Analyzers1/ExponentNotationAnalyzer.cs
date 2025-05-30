using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers1;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ExponentNotationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "ES0001";

    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.ES0001Title),
        Resources.ResourceManager, typeof(Resources));

    private static readonly LocalizableString MessageFormat =
        new LocalizableResourceString(nameof(Resources.ES0001MessageFormat), Resources.ResourceManager,
            typeof(Resources));

    private static readonly LocalizableString Description =
        new LocalizableResourceString(nameof(Resources.ES0001Description), Resources.ResourceManager,
            typeof(Resources));

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
        if (context.Node is not LiteralExpressionSyntax literalExpression)
            return;
        if (!literalExpression.Token.IsKind(SyntaxKind.NumericLiteralToken))
            return;
        if (literalExpression.Token.Value is not 1000000000)
            return;
        if (literalExpression.Token.Text == "1e9")
            return;

        var diagnostic = Diagnostic.Create(Rule,
            literalExpression.GetLocation(),
            literalExpression.Token.Text);
        context.ReportDiagnostic(diagnostic);
    }
}