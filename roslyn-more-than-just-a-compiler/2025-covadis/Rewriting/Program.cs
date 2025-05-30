using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

const string sourceFilePath = "/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/Solution2/Gigasecond.cs";
var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFilePath));

var root = await syntaxTree.GetRootAsync();

root = new RemoveEmptyStatements().Visit(root);
root = new UseVarRewriter().Visit(root);
root = new UseExponentNotation().Visit(root);

root = root.NormalizeWhitespace();

File.WriteAllText(sourceFilePath, root.ToFullString());

internal sealed class RemoveEmptyStatements : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitEmptyStatement(EmptyStatementSyntax node)
    {
        return null;
    }
}

internal sealed class UseExponentNotation : CSharpSyntaxRewriter
{
    public override SyntaxToken VisitToken(SyntaxToken token)
    {
        if (token.IsKind(SyntaxKind.NumericLiteralToken) && 
            token.Value is 1000000000 &&
            token.Text != "1e9")
            return SyntaxFactory.Literal(
                token.LeadingTrivia,
                "1e9",
                1000000000,
                token.TrailingTrivia
            );
        
        return base.VisitToken(token);
    }
}

internal sealed class UseVarRewriter : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitVariableDeclaration(VariableDeclarationSyntax node)
    {
        if (node.Type.IsVar)
            return base.VisitVariableDeclaration(node);
        
        return base.VisitVariableDeclaration(
            node.WithType(
                SyntaxFactory.IdentifierName("var").WithTriviaFrom(node.Type)
            ));
    }
}
