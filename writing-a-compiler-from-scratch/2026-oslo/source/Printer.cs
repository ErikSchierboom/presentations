namespace BoomScript;

public static class Printer
{
    public static void Print(Tree tree)
    {
        for (var i = 0; i < tree.Statements.Count; i++)
            PrintNode(tree.Statements[i], "", i == tree.Statements.Count - 1);
    }

    private static void PrintNode(object node, string prefix, bool isLast)
    {
        Console.WriteLine($"{prefix}{(isLast ? "└── " : "├── ")}{GetLabel(node)}");

        var children = GetChildren(node);
        var childPrefix = prefix + (isLast ? "    " : "│   ");
        for (var i = 0; i < children.Count; i++)
            PrintNode(children[i], childPrefix, i == children.Count - 1);
    }

    private static string GetLabel(object node) => node switch
    {
        AssignmentStatement assignment => $"Assignment: {assignment.VariableName}",
        ExpressionStatement => "Expression",
        NumberLiteralExpression number => $"Number: {number.Value}",
        VariableExpression variable => $"Variable: {variable.Name}",
        BinaryExpression binary => $"Binary: {binary.Operator}",
        _ => node.ToString() ?? string.Empty
    };

    private static IReadOnlyList<object> GetChildren(object node) => node switch
    {
        AssignmentStatement assignment => [assignment.Value],
        ExpressionStatement expressionStatement => [expressionStatement.Expression],
        BinaryExpression binary => [binary.Left, binary.Right],
        _ => []
    };
}
