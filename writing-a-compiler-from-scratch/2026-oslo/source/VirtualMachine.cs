namespace BoomScript;

public class VirtualMachine(List<Instruction> instructions)
{
    private readonly Stack<int> _stack = new();
    private readonly int[] _registers = new int[256];
    
    public int Run()
    {
        foreach (var instruction in instructions)
        {
            switch (instruction)
            {
                case LoadIntInstruction loadIntInstruction:
                    _stack.Push(loadIntInstruction.Value);
                    break;
                case LoadVarInstruction loadVarInstruction:
                    _stack.Push(_registers[loadVarInstruction.Index]);
                    break;
                case StoreVarInstruction storeVarInstruction:
                    _registers[storeVarInstruction.Index] = _stack.Pop();
                    break;
                case AddInstruction:
                    var addRight = _stack.Pop();
                    var addLeft = _stack.Pop();
                    _stack.Push(addLeft + addRight);
                    break;
                case MulInstruction:
                    var mulRight = _stack.Pop();
                    var mulLeft = _stack.Pop();
                    _stack.Push(mulLeft * mulRight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(instruction));
            }
        }

        return _stack.Pop();
    }
}

public abstract record Instruction;
public record LoadIntInstruction(int Value) : Instruction;
public record LoadVarInstruction(int Index) : Instruction;
public record StoreVarInstruction(int Index) : Instruction;
public record AddInstruction : Instruction;
public record MulInstruction : Instruction;
