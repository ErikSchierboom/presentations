using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerator.Tests;

public class AdditionalTextFile(string path, string text) : AdditionalText
{
    public override SourceText GetText(CancellationToken cancellationToken = new()) => SourceText.From(text);

    public override string Path => path;
}