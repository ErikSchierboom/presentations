using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

const string sourceFilePath = @"/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/Solutions/TwoFer.cs";
var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFilePath));

var root = await syntaxTree.GetRootAsync();

root = new RemoveEmptyStatements().Visit(root);
root = new UseVarRewriter().Visit(root);
root = new RemoveComments().Visit(root);
root = new AddBracesToIfElse().Visit(root);
root = new SimplifyBooleanExpression().Visit(root);
root = new UseFileScopedNamespace().Visit(root);

root = root.NormalizeWhitespace();

File.WriteAllText(sourceFilePath, root.ToFullString());

internal sealed class RemoveEmptyStatements : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitEmptyStatement(EmptyStatementSyntax node)
    {
        return null;
    }
}

internal sealed class UseFileScopedNamespace : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        return Visit(SyntaxFactory.FileScopedNamespaceDeclaration(
            node.AttributeLists, node.Modifiers, node.Name, node.Externs, node.Usings, node.Members)
            .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.CarriageReturnLineFeed));
    }
}

internal sealed class SimplifyBooleanExpression : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitBinaryExpression(BinaryExpressionSyntax node)
    {
        if (node.IsKind(SyntaxKind.EqualsExpression))
        {
            if (node.Right.IsKind(SyntaxKind.TrueLiteralExpression))
                return Visit(node.Left);
            
            if (node.Right.IsKind(SyntaxKind.FalseLiteralExpression))
                return Visit(
                    SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, node.Left));
        }
        
        return base.VisitBinaryExpression(node);
    }
}

internal sealed class AddBracesToIfElse : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitIfStatement(IfStatementSyntax node)
    {
        if (node.Statement is BlockSyntax)
            return base.VisitIfStatement(node);
        
        return base.VisitIfStatement(node.WithStatement(
                SyntaxFactory.Block(node.Statement)));
    }

    public override SyntaxNode? VisitElseClause(ElseClauseSyntax node)
    {
        if (node.Statement is BlockSyntax)
            return base.VisitElseClause(node);
        
        return base.VisitElseClause(node.WithStatement(
            SyntaxFactory.Block(node.Statement)));
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

internal sealed class RemoveComments : CSharpSyntaxRewriter
{
    public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
    {
        if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
            return default;
        
        return base.VisitTrivia(trivia);
    }
}
