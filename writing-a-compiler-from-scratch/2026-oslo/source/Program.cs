const string code = """
                    var x = 45;
                    var y = 1 + 2 * 3;
                    x + y
                    """;

var tokens = new Scanner(code).Scan();
foreach (var token in tokens)
{
    Console.WriteLine(token);
}

public enum TokenType
{
    Var,
    Identifier,
    Integer,
    Equal,
    Semicolon,
    Plus,
    Star
}

public record Token(TokenType Type, string Value);

public class Scanner(string code)
{
    public List<Token> Scan()
    {
        var position = 0;
        var tokens = new List<Token>();
        
        while (position < code.Length)
        {
            switch (code[position])
            {
                case ' ' or '\t' or '\n' or '\r':
                    position++;
                    continue;
                case '=':
                    position++;
                    tokens.Add(new Token(TokenType.Equal, "="));
                    break;
                case '*':
                    position++;
                    tokens.Add(new Token(TokenType.Star, "*"));
                    break;
                case '+':
                    position++;
                    tokens.Add(new Token(TokenType.Plus, "+"));
                    break;
                case ';':
                    position++;
                    tokens.Add(new Token(TokenType.Semicolon, ";"));
                    break;
                case >= 'a' and <= 'z':
                    var start = position;
                    while (position < code.Length && code[position] >= 'a' && code[position] <= 'z')
                    {
                        position++;
                    }

                    var lexeme = code.Substring(start, position - start);
                    if (lexeme == "var")
                        tokens.Add(new Token(TokenType.Var, lexeme));
                    else
                        tokens.Add(new Token(TokenType.Identifier, lexeme));
                    break;
                case >= '0' and <= '9':
                    var integerStart = position;
                    while (position < code.Length && code[position] >= '0' && code[position] <= '9')
                    {
                        position++;
                    }
                    tokens.Add(new Token(TokenType.Integer, code.Substring(integerStart, position - integerStart)));
                    break;
                default:
                    throw new Exception($"Unexpected character: {code[position]} at position {position}");
            }
        }

        return tokens;
    }
}