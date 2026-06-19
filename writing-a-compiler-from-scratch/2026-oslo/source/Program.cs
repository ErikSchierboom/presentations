// 1. LEXING/SCANNING: converting plaintext into tokens
// 2. PARSING: convert tokens into a syntax tree (abstract vs concrete)
// 3. COMPILATION: convert syntax tree into bytecode/machine code
// 4. VIRTUAL MACHINE: run the bytecode

const string code = """
                    var x = 4;
                    var y = 1 + 2 * 3;
                    x + y
                    """;

var lexer = new Lexer(code);
var tokens = lexer.Lex();

var parser = new Parser(tokens);
var statements = parser.Parse();

var compiler = new Compiler();
var instructions = compiler.Compile(statements);
var runtime = new VirtualMachine();
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
    public abstract void Execute(VirtualMachine virtualMachine);
}

internal record LoadIntInstruction(int Value) : Instruction
{
    public override void Execute(VirtualMachine virtualMachine)
    {
        virtualMachine.Push(Value);
    }
}

internal record LoadLocalInstruction(int Index) : Instruction
{
    public override void Execute(VirtualMachine virtualMachine)
    {
        var value = virtualMachine.GetLocal(Index);
        virtualMachine.Push(value);
    }
}

internal record StoreLocalInstruction(int Index) : Instruction
{
    public override void Execute(VirtualMachine virtualMachine)
    {
        var value = virtualMachine.Pop();
        virtualMachine.SetLocal(Index, value);
    }
}

internal record AddInstruction : Instruction
{
    public override void Execute(VirtualMachine virtualMachine)
    {
        var right = virtualMachine.Pop();
        var left = virtualMachine.Pop();
        virtualMachine.Push(left + right);
    }
}

internal record MulInstruction : Instruction
{
    public override void Execute(VirtualMachine virtualMachine)
    {
        var right = virtualMachine.Pop();
        var left = virtualMachine.Pop();
        virtualMachine.Push(left * right);
    }
}

internal class VirtualMachine
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
