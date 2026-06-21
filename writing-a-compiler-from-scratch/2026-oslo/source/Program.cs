using BoomScript;

const string code = """
                    var x = 3;
                    var result = 1 * 2 + x;
                    result + 10
                    """;

var tokens = new Scanner(code).Scan();
var tree = new Parser(tokens).Parse();
var instructions = new Compiler(tree).Compile();
var result = new VirtualMachine(instructions).Run();
Console.WriteLine(result);

public class Compiler(Tree tree)
{
    private readonly List<Instruction> _instructions = new();
    private readonly Dictionary<string, int> _variableToIndex = new(capacity: 256);

    public List<Instruction> Compile()
    {
        // TODO: implement compiler
        
        return _instructions;
    }
}

public abstract record Instruction;
public record LoadIntInstruction(int Value) : Instruction;
public record LoadVarInstruction(int Index) : Instruction;
public record StoreVarInstruction(int Index) : Instruction;
public record AddInstruction : Instruction;
public record MulInstruction : Instruction;

public class VirtualMachine(List<Instruction> instructions)
{
    public Stack<int> Stack { get; } = new();
    public int[] Variables { get; } = new int[256];
    
    public int Run()
    {   
        throw new NotImplementedException();
    }
}
