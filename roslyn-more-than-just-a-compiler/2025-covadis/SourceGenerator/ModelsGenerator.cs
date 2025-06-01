using Microsoft.CodeAnalysis;

namespace SourceGenerator;

[Generator]
public class ModelsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // TODO: get the additional text files
        // TODO: register source output
    }
}
