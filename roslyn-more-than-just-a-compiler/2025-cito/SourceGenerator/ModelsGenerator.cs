using System;
using System.Collections.Immutable;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceGenerator;

[Generator]
public class ModelsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // TODO: gather additional files
        // TODO: register source output
    }

    private static void GenerateSource(SourceProductionContext context, ImmutableArray<AdditionalText> additionalTexts)
    {
        throw new NotImplementedException();
    }
}
