namespace BoomScript;

public class Parser(List<Token> tokens)
{
    private int _position;
    
    public Tree Parse()
    {
        var statements = new List<Statement>();
        
        while (Current.Type != TokenType.Eof)
            statements.Add(ParseStatement());
        
        return new Tree(statements);
    }

    private Statement ParseStatement()
    {
        if (Match(TokenType.Var))
            return ParseAssignmentStatement();
        
        return ParseExpressionStatement();
    }

    private AssignmentStatement ParseAssignmentStatement()
    {
        Consume(TokenType.Ident);
        var name = Previous.Lexeme;
        Consume(TokenType.Equal);
        var value = ParseExpression();
        Consume(TokenType.Semicolon);
        return new AssignmentStatement(name, value);
    }

    private ExpressionStatement ParseExpressionStatement()
    {
        var value = ParseExpression();
        Consume(TokenType.Semicolon);
        return new ExpressionStatement(value);
    }

    private Expression ParseExpression()
    {
        return ParseLowPrecedenceExpression();
    }

    private Expression ParseLowPrecedenceExpression()
    {
        var left = ParseNormalPrecedenceExpression();
        
        while (Match(TokenType.Plus))
        {
            var operatorType = Previous.Type;
            var right = ParseNormalPrecedenceExpression();
            left = new BinaryExpression(left, operatorType, right);
        }

        return left;
    }

    private Expression ParseNormalPrecedenceExpression()
    {
        var left = ParseHighPrecedenceExpression();
        
        while (Match(TokenType.Star))
        {
            var operatorType = Previous.Type;
            var right = ParseHighPrecedenceExpression();
            left = new BinaryExpression(left, operatorType, right);
        }

        return left;
    }

    private Expression ParseHighPrecedenceExpression()
    {
        if (Match(TokenType.Num))
            return new NumberLiteralExpression(int.Parse(Previous.Lexeme));
        
        if (Match(TokenType.Ident))
            return new VariableExpression(Previous.Lexeme);
            
        throw new InvalidOperationException($"Unexpected token: {Current.Type}");
    }

    private bool Match(TokenType type)
    {
        if (Current.Type != type)
            return false;
        
        _position++;
        return true;
    }
    
    private void Consume(TokenType type)
    {
        if (Current.Type != type)
            throw new Exception($"Expected {type}, got {Current.Type}.");
        
        _position++;
    }
    
    private Token Current => tokens[_position];
    private Token Previous => tokens[_position - 1];
}

public record Tree(List<Statement> Statements);

public abstract record Statement;
public record AssignmentStatement(string VariableName, Expression Value) : Statement;
public record ExpressionStatement(Expression Expression) : Statement;

public abstract record Expression;
public record NumberLiteralExpression(int Value) : Expression;
public record BinaryExpression(Expression Left, TokenType Operator, Expression Right) : Expression;
public record VariableExpression(string Name) : Expression;
