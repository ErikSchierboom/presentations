using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

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
        
        // TODO: register invocation operation callback
    }
}