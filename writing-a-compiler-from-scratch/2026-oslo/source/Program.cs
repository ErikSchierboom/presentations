using BoomScript;

const string code = """
                    var x = 3;
                    var result = 1 * 2 + x;
                    result + 10;
                    """;

var tokens = new Scanner(code).Scan();
var tree = new Parser(tokens).Parse();
foreach (var statement in tree.Statements)
{
    Console.WriteLine(statement);
}

public record Tree(List<Statement> Statements);

public abstract record Statement;
public record AssignmentStatement(string VariableName, Expression Value) : Statement;
public record ExpressionStatement(Expression Expression) : Statement;

public abstract record Expression;
public record NumberLiteralExpression(int Value) : Expression;
public record BinaryExpression(Expression Left, TokenType Operator, Expression Right) : Expression;
public record VariableExpression(string Name) : Expression;

public class Parser(List<Token> tokens)
{
    private int _position = 0;
    
    private Token Current => tokens[_position];
    private Token Previous => tokens[_position - 1];
    
    public Tree Parse()
    {
        var statements = new List<Statement>();
        
        // TODO: parse statements
        
        return new Tree(statements);
    }

    private void Consume(TokenType expected)
    {
        if (Current.Type != expected)
            throw new InvalidOperationException($"Expected {expected}, but found {Current.Type}");

        _position++;
    }

    private bool Match(TokenType expected)
    {
        if (Current.Type != expected)
            return false;

        _position++;
        return true;
    }
}
