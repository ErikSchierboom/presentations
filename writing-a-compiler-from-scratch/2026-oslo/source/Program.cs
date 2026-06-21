using BoomScript;

const string code = """
                    var x = 3;
                    var result = 1 * 2 + x;
                    result + 10
                    """;

var tokens = new Scanner(code).Scan();
var tree = new Parser(tokens).Parse();
var instructions = new Compiler().Compile(tree);
var result = new VirtualMachine().Run(instructions);
Console.WriteLine(result);

public class Compiler
{
    public List<Instruction> Compile(Tree tree)
    {
        throw new NotImplementedException();
    }
}

public abstract record Instruction;
public record LoadIntInstruction(int Value) : Instruction;
public record LoadVarInstruction(int Index) : Instruction;
public record StoreVarInstruction(int Index) : Instruction;
public record AddInstruction() : Instruction;
public record MulInstruction() : Instruction;

public class VirtualMachine
{
    public Stack<int> Stack { get; } = new();
    public int[] Variables { get; } = new int[256];
    
    public int Run(List<Instruction> instructions)
    {   
        throw new NotImplementedException();
    }
}

