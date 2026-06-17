// 1. define constant code string "2"
// 2. define lexer stub
// 3. define TokenKind enum
// 4. add position variable and add while loop
// 5. add switch case for >= '0' and <= '9' character and add token to list
// 6. convert number to "23" and add inner while loop

// LEXING/SCANNING: converting plaintext into tokens
// PARSING: convert tokens into a syntax tree (abstract vs concrete)
// COMPILATION: convert syntax tree into bytecode/machine code
// OPTIMIZER: simplify the syntax tree or bytecode/machine code
// RUNTIME: run the bytecode (if bytecode was produced)

const string code = """
                    var x = 1;
                    var y = 2;
                    var z = x + y * 4;
                    z + 1;
                    """;

var lexer = new Lexer(code);
var tokens = lexer.Lex();

var parser = new Parser(tokens);
var statements = parser.Parse();

var interpreter = new Interpreter();
Console.WriteLine("Interpreted:");
Console.WriteLine(interpreter.Evaluate(statements));

var compiler = new Compiler();
var instructions = compiler.Compile(statements);
var runtime = new Runtime();
Console.WriteLine("Compiled:");
Console.WriteLine(runtime.Run(instructions));

public enum TokenKind
{
    Number,
    Identifier,
    
    Semicolon,
    Plus,
    Star,
    Equal,
    
    Var,

    EndOfFile
}

public record Token(TokenKind Kind, string Text);

public class Lexer(string source)
{
    public List<Token> Lex()
    {
        var tokens = new List<Token>();
        var current = 0;

        while (current < source.Length)
        {
            var start = current;
            
            switch (source[current])
            {
                case '+':
                    current++;
                    tokens.Add(new Token(TokenKind.Plus, "+"));
                    break;
                case '*':
                    current++;
                    tokens.Add(new Token(TokenKind.Star, "*"));
                    break;
                case ';':
                    current++;
                    tokens.Add(new Token(TokenKind.Semicolon, ";"));
                    break;
                case '=':
                    current++;
                    tokens.Add(new Token(TokenKind.Equal, "="));
                    break;
                case ' ' or '\t' or '\r' or '\n':
                    current++;
                    break;
                case >= '0' and <= '9':
                    while (current < source.Length && char.IsDigit(source[current]))
                        current++;
                    
                    tokens.Add(new Token(TokenKind.Number, source[start..current]));
                    break;
                case >= 'a' and <= 'z' or >= 'A' and <= 'Z':
                    while (current < source.Length && char.IsAsciiLetterOrDigit(source[current]))
                        current++;
                    
                    var identifier = source[start..current];
                    if (identifier == "var")
                        tokens.Add(new Token(TokenKind.Var, identifier));
                    else
                        tokens.Add(new Token(TokenKind.Identifier, identifier));
                    break;
                default:
                    throw new InvalidOperationException("Unexpected character");
            }
        }
        
        tokens.Add(new Token(TokenKind.EndOfFile, ""));
        return tokens;
    }
}

public abstract record Node;

public abstract record Statement : Node;
public record AssignmentStatement(Token Name, Expression Initializer) : Statement;
public record ExpressionStatement(Expression Expression) : Statement;

public abstract record Expression : Node;
public record VariableExpression(Token Name) : Expression;
public record LiteralExpression(Token Value) : Expression;
public record BinaryExpression(Expression Left, Token Operator, Expression Right) : Expression;

public class Parser(List<Token> tokens)
{
    private int _position;

    public List<Statement> Parse()
    {
        var statements = new List<Statement>();

        while (!IsEndOfFile)
            statements.Add(ParseStatement());
        
        return statements;
    }

    private bool IsEndOfFile => Token.Kind == TokenKind.EndOfFile;

    private Statement ParseStatement()
    {
        if (Match(TokenKind.Var))
            return ParseAssignmentStatement();

        return ParseExpressionStatement();
    }

    private AssignmentStatement ParseAssignmentStatement()
    {
        Consume(TokenKind.Identifier);
        var name = PreviousToken;
        Consume(TokenKind.Equal);
        var initializer = ParseExpression();
        Consume(TokenKind.Semicolon);
        
        return new AssignmentStatement(name, initializer);
    }

    private ExpressionStatement ParseExpressionStatement()
    {
        var expression = ParseExpression();
        Consume(TokenKind.Semicolon);

        return new ExpressionStatement(expression);
    }

    private Expression ParseExpression()
    {
        var expr = Term();

        return Match(TokenKind.Star) 
            ? new BinaryExpression(expr, PreviousToken, Term()) 
            : expr;
    }

    private Expression Term()
    {
        var expr = Primary();   
        
        return Match(TokenKind.Plus) 
            ? new BinaryExpression(expr,  PreviousToken, Primary()) 
            : expr;
    }
    
    private Expression Primary()
    {
        if (Match(TokenKind.Number))
            return new LiteralExpression(PreviousToken);
        
        if (Match(TokenKind.Identifier))
            return new VariableExpression(PreviousToken);
        
        throw new InvalidOperationException("Unexpected token");
    }

    private bool Match(TokenKind kind)
    {
        if (Token.Kind != kind)
            return false;

        _position++;
        return true;

    }
    
    private void Consume(TokenKind kind)
    {
        if (Token.Kind != kind)
            throw new InvalidOperationException($"Expected '{kind}' token");
        
        _position++;
    }

    private Token Token => tokens[_position];
    private Token PreviousToken => tokens[_position - 1];
}

internal class Interpreter
{
    private readonly Dictionary<string, int> _variables = new();

    public int Evaluate(List<Statement> statements)
    {
        var result = -1;
        
        foreach (var statement in statements)
            result = Evaluate(statement);
        
        return result;
    }
    
    private int Evaluate(Statement statement)
    {
        switch (statement)
        {
            case AssignmentStatement assignmentStatement:
                var value = Evaluate(assignmentStatement.Initializer);
                _variables[assignmentStatement.Name.Text] = value;
                return value;
            case ExpressionStatement expressionStatement:
                return Evaluate(expressionStatement.Expression);
            default:
                throw new ArgumentOutOfRangeException(nameof(statement));
        }
    }

    private int Evaluate(Expression expression) =>
        expression switch
        {
            BinaryExpression { Operator.Kind: TokenKind.Plus } binExpr => Evaluate(binExpr.Left) +
                                                                          Evaluate(binExpr.Right),
            BinaryExpression { Operator.Kind: TokenKind.Star } binExpr => Evaluate(binExpr.Left) *
                                                                          Evaluate(binExpr.Right),
            LiteralExpression { Value.Kind: TokenKind.Number } litEpr => int.Parse(litEpr.Value.Text),
            VariableExpression variableExpression => _variables[variableExpression.Name.Text],
            _ => throw new InvalidOperationException("Unexpected expression")
        };
}

internal class Compiler
{
    private readonly Dictionary<string, int> _variableToLocalIndex = new();
    private readonly List<Instruction> _instructions = new();

    public List<Instruction> Compile(List<Statement> statements)
    {
        foreach (var statement in statements)
            Compile(statement);
        
        return _instructions;
    }

    private void Compile(Statement statement)
    {
        switch (statement)
        {
            case AssignmentStatement assignmentStatement:
                _variableToLocalIndex[assignmentStatement.Name.Text] = _variableToLocalIndex.Count;
                Compile(assignmentStatement.Initializer);
                _instructions.Add(new StoreLocalInstruction(_variableToLocalIndex[assignmentStatement.Name.Text]));
                break;
            case ExpressionStatement expressionStatement:
                Compile(expressionStatement.Expression);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(statement));
        }
    }

    private void Compile(Expression expression)
    {
        switch (expression)
        {
            case BinaryExpression binaryExpression:
                Compile(binaryExpression.Left);
                Compile(binaryExpression.Right);
                
                switch (binaryExpression.Operator.Kind)
                {
                    case TokenKind.Plus:
                        _instructions.Add(new AddInstruction());
                        break;
                    case TokenKind.Star:
                        _instructions.Add(new MulInstruction());
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected operator token");
                }
                break;
            case LiteralExpression numericLiteralExpression:
                _instructions.Add(new LoadIntInstruction(int.Parse(numericLiteralExpression.Value.Text)));
                break;
            case VariableExpression variableExpression:
                _instructions.Add(new LoadLocalInstruction(_variableToLocalIndex[variableExpression.Name.Text]));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(expression));
        }
    }
}

internal abstract record Instruction
{
    public abstract void Execute(Runtime runtime);
}

internal record LoadIntInstruction(int Value) : Instruction
{
    public override void Execute(Runtime runtime)
    {
        runtime.Push(Value);
    }
}

internal record LoadLocalInstruction(int Index) : Instruction
{
    public override void Execute(Runtime runtime)
    {
        var value = runtime.GetLocal(Index);
        runtime.Push(value);
    }
}

internal record StoreLocalInstruction(int Index) : Instruction
{
    public override void Execute(Runtime runtime)
    {
        var value = runtime.Pop();
        runtime.SetLocal(Index, value);
    }
}

internal record AddInstruction : Instruction
{
    public override void Execute(Runtime runtime)
    {
        var right = runtime.Pop();
        var left = runtime.Pop();
        runtime.Push(left + right);
    }
}

internal record MulInstruction : Instruction
{
    public override void Execute(Runtime runtime)
    {
        var right = runtime.Pop();
        var left = runtime.Pop();
        runtime.Push(left * right);
    }
}

internal class Runtime
{
    private readonly Stack<int> _stack = new();
    private readonly int[] _locals = new int[256];
    
    public int Run(List<Instruction> instructions)
    {   
        foreach (var instruction in instructions)
            instruction.Execute(this);

        return _stack.Pop();
    }
    
    public void Push(int value) => _stack.Push(value);
    public int Pop() => _stack.Pop();
    
    public int GetLocal(int index) => _locals[index];
    public void SetLocal(int index, int value) => _locals[index] = value;
}
