namespace BoomScript;

public class Compiler(Tree tree)
{
    private readonly List<Instruction> _instructions = new();
    private readonly Dictionary<string, int> _variableToIndex = new(capacity: 256);
    
    public List<Instruction> Compile()
    {
        foreach (var statement in tree.Statements)
            CompileStatement(statement);

        return _instructions;
    }

    private void CompileStatement(Statement statement)
    {
        switch (statement)
        {
            case AssignmentStatement assignmentStatement:
                CompileExpression(assignmentStatement.Value);
                
                if (!_variableToIndex.ContainsKey(assignmentStatement.VariableName))
                    _variableToIndex[assignmentStatement.VariableName] = _variableToIndex.Count;

                _instructions.Add(new StoreVarInstruction(_variableToIndex[assignmentStatement.VariableName]));
                break;
            case ExpressionStatement expressionStatement:
                CompileExpression(expressionStatement.Expression);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(statement));
        }
    }

    private void CompileExpression(Expression expression)
    {
        switch (expression)
        {
            case BinaryExpression binaryExpression:
                CompileExpression(binaryExpression.Left);
                CompileExpression(binaryExpression.Right);

                switch (binaryExpression.Operator)
                {
                    case TokenType.Plus:
                        _instructions.Add(new AddInstruction());
                        break;
                    case TokenType.Star:
                        _instructions.Add(new MulInstruction());
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported binary operator {binaryExpression.Operator}");
                }
                break;
            case NumberLiteralExpression numberLiteralExpression:
                _instructions.Add(new LoadIntInstruction(numberLiteralExpression.Value));
                break;
            case VariableExpression variableExpression:
                _instructions.Add(new LoadVarInstruction(_variableToIndex[variableExpression.Name]));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(expression));
        }
    }
}
