using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

var workspace = MSBuildWorkspace.Create();
var project = await workspace.OpenProjectAsync(
    "/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/Solutions/Solutions.csproj");
var solution = project.Solution;

var document = project.Documents.First(document => document.Name == "Leap.cs");
var semanticModel = await document.GetSemanticModelAsync();
var documentRoot = await document.GetSyntaxRootAsync();

var usingDirectiveSyntaxes = documentRoot.DescendantNodes().OfType<UsingDirectiveSyntax>();

var updatedRoot = new UseVarRewriter().Visit(documentRoot);
updatedRoot = new RemoveEmptyStatements().Visit(updatedRoot);
updatedRoot = new RemoveComments().Visit(updatedRoot);
updatedRoot = new AddBracesToIfElse().Visit(updatedRoot);
updatedRoot = new SimplifyBooleanExpression().Visit(updatedRoot);
updatedRoot = new UseFileScopedNamespace().Visit(updatedRoot);

updatedRoot = updatedRoot.NormalizeWhitespace();

document = document.WithSyntaxRoot(updatedRoot);
workspace.TryApplyChanges(document.Project.Solution);

// documentRoot = await document.GetSyntaxRootAsync();
// semanticModel = await document.GetSemanticModelAsync();
//
// workspace.TryApplyChanges(document.Project.Solution);

// File.WriteAllText(document.FilePath, updatedRoot.NormalizeWhitespace().ToString());

class RemoveEmptyStatements : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitEmptyStatement(EmptyStatementSyntax node)
    {
        return null;
    }
}

class UseFileScopedNamespace : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        return base.Visit(SyntaxFactory.FileScopedNamespaceDeclaration(
            node.AttributeLists, node.Modifiers, node.Name, node.Externs, node.Usings, node.Members)
            .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.CarriageReturnLineFeed));
    }
}

class SimplifyBooleanExpression : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitBinaryExpression(BinaryExpressionSyntax node)
    {
        if (node.IsKind(SyntaxKind.EqualsExpression))
        {
            if (node.Right.IsKind(SyntaxKind.TrueLiteralExpression))
                return base.Visit(node.Left);
            
            if (node.Right.IsKind(SyntaxKind.FalseLiteralExpression))
                return base.Visit(
                    SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, node.Left));
        }
        
        return base.VisitBinaryExpression(node);
    }
}

class AddBracesToIfElse : CSharpSyntaxRewriter
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

class UseVarRewriter : CSharpSyntaxRewriter
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

class RemoveComments : CSharpSyntaxRewriter
{
    public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
    {
        if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
            return default;
        
        return base.VisitTrivia(trivia);
    }
}

// var underscoreVariableSymbol = documentRoot.DescendantNodes()
//     .OfType<VariableDeclaratorSyntax>()
//     .Where(variableDeclarator => variableDeclarator.Identifier.Text.StartsWith('_'))
//     .Select(variableDeclarator => semanticModel.GetDeclaredSymbol(variableDeclarator))
//     .Single();
//
// var newSolution = await Renamer.RenameSymbolAsync(project.Solution, underscoreVariableSymbol, new SymbolRenameOptions(),
//     underscoreVariableSymbol.Name[1..]);  
// workspace.TryApplyChanges(newSolution);
