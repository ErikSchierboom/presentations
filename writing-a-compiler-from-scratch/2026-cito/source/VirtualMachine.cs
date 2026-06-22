namespace BoomScript;

public class VirtualMachine(List<Instruction> instructions)
{
    public Stack<int> Stack { get; } = new();
    public int[] Variables { get; } = new int[256];
    
    public int Run()
    {
        foreach (var instruction in instructions)
            instruction.Execute(this);

        return Stack.Pop();
    }
}

public abstract record Instruction
{
    public abstract void Execute(VirtualMachine vm);
}

public record LoadIntInstruction(int Value) : Instruction
{
    public override void Execute(VirtualMachine vm)
    {
        vm.Stack.Push(Value);
    }
}

public record LoadVarInstruction(int Index) : Instruction
{
    public override void Execute(VirtualMachine vm)
    {
        vm.Stack.Push(vm.Variables[Index]);
    }
}

public record StoreVarInstruction(int Index) : Instruction
{
    public override void Execute(VirtualMachine vm)
    {
        vm.Variables[Index] = vm.Stack.Pop();
    }
}

public record AddInstruction : Instruction
{
    public override void Execute(VirtualMachine vm)
    {
        var right = vm.Stack.Pop();
        var left = vm.Stack.Pop();
        vm.Stack.Push(left + right);
    }
}

public record MulInstruction : Instruction
{
    public override void Execute(VirtualMachine vm)
    {
        var right = vm.Stack.Pop();
        var left = vm.Stack.Pop();
        vm.Stack.Push(left * right);
    }
}
