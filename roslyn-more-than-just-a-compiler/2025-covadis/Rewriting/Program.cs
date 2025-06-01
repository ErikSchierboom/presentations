using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var sourceCodeFilePath = Path.GetFullPath("../../../../Rewriting.Example/Gigasecond.cs");
var sourceCode = File.ReadAllText(sourceCodeFilePath);

var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
var root = await syntaxTree.GetRootAsync();
root = new RemoveEmptyStatements().Visit(root);
root = new UseExponentNotation().Visit(root);
root = root.NormalizeWhitespace();

File.WriteAllText(sourceCodeFilePath, root.ToFullString());

Console.WriteLine("Rewritten");

internal class RemoveEmptyStatements : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitEmptyStatement(EmptyStatementSyntax node)
    {
        return null;
    }
}

internal class UseExponentNotation : CSharpSyntaxRewriter
{
    public override SyntaxToken VisitToken(SyntaxToken token)
    {
        if (token.IsKind(SyntaxKind.NumericLiteralToken) &&
            token.Value is 1_000_000_000 &&
            token.Text != "1e9")
            return SyntaxFactory.Literal("1e9", 1_000_000_000);
            
        return base.VisitToken(token);
    }
}
