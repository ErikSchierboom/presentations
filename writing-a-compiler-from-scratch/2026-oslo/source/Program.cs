using BoomScript;

const string code = """
                    var x = 3;
                    var result = 1 + 2 * x;
                    result + 10;
                    """;

var tokens = new Scanner("3;").Scan();
var tree = new Parser(tokens).Parse();
var instructions = new Compiler(tree).Compile();
var result = new VirtualMachine(instructions).Run();
Console.WriteLine(result);

public abstract record Instruction;
public record LoadIntInstruction(int Value) : Instruction;
public record LoadVarInstruction(byte Index) : Instruction;
public record StoreVarInstruction(byte Index) : Instruction;
public record AddInstruction : Instruction;
public record MulInstruction : Instruction;

public class Compiler(Tree tree)
{
    private readonly List<Instruction> _instructions = new();
    private readonly Dictionary<string, byte> _variableToIndex = new(capacity: 256);

    public List<Instruction> Compile()
    {
        // TODO: compile statements
        
        return _instructions;
    }

    private void Compile(Statement statement)
    {
        throw new NotImplementedException();
    }
    
    private void Compile(Expression expression)
    {
        throw new NotImplementedException();
    }
}

public class VirtualMachine(List<Instruction> instructions)
{
    private readonly Stack<int> _stack = new();
    private readonly int[] _registers = new int[256];
    
    public int Run()
    {   
        throw new NotImplementedException();
    }
}
