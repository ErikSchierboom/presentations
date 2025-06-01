using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class AddSecondsAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "ES0002";
    private const string Category = "Usage";
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.ES0002Title), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.ES0002MessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.ES0002Description), Resources.ResourceManager, typeof(Resources));

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(compilationStartContext =>
        {
            var dateTimeType = compilationStartContext.Compilation.GetSpecialType(SpecialType.System_DateTime);
            var addMillisecondsMethod = dateTimeType.GetMembers("AddMilliseconds").OfType<IMethodSymbol>().First();
            
            compilationStartContext.RegisterOperationAction(
                operationAnalysisContext => AnalyzeOperation(operationAnalysisContext, addMillisecondsMethod), 
                OperationKind.Invocation);
        });
    }

    private static void AnalyzeOperation(OperationAnalysisContext context, IMethodSymbol addMillisecondsMethod)
    {
        if (context.Operation is not IInvocationOperation invocationOperation ||
            !invocationOperation.TargetMethod.Equals(addMillisecondsMethod, SymbolEqualityComparer.Default) ||
            invocationOperation.Syntax is not InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax memberAccessExpression })
            return;
        
        var diagnostic = Diagnostic.Create(Rule,
            invocationOperation.Syntax.GetLocation(),
            memberAccessExpression.Name.Identifier.Text);
        context.ReportDiagnostic(diagnostic);
    }
}