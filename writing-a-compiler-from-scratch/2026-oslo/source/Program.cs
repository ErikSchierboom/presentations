using BoomScript;

const string code = """
                    var x = 3;
                    var result = 1 * 2 + x;
                    result + 10
                    """;

var tokens = new Scanner(code).Scan();
var tree = new Parser(tokens).Parse();
foreach (var statement in tree.Statements)
{
    Console.WriteLine(statement);
}

public class Parser(List<Token> tokens)
{
    private int _position = 0;
    
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

    private Statement ParseAssignmentStatement()
    {
        Consume(TokenType.Ident);
        var name = Previous.Lexeme;
        Consume(TokenType.Equal);
        var value = ParseExpression();
        Consume(TokenType.Semicolon);
        return new AssignmentStatement(name, value);
    }

    private Statement ParseExpressionStatement()
    {
        var value = ParseExpression();
        return new ExpressionStatement(value);
    }

    private Expression ParseExpression()
    {
        return ParseTerm();
    }

    private Expression ParseTerm()
    {
        var left = ParseFactor();
        if (!Match(TokenType.Plus))
            return left;

        var right = ParseFactor();
        return new BinaryExpression(left, TokenType.Plus, right);
    }

    private Expression ParseFactor()
    {
        var left = ParsePrimary();
        if (!Match(TokenType.Star))
            return left;

        var right = ParsePrimary();
        return new BinaryExpression(left, TokenType.Star, right); 
    }

    private Expression ParsePrimary()
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
