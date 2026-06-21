const string code = """
                    var x = 3;
                    var result = 1 * 2 + x;
                    result + 10;
                    """;

var tokens = new Scanner(code).Scan();
foreach (var token in tokens)
{
    Console.WriteLine(token);
}

public record Token(TokenType Type, string Text);

public enum TokenType
{
    Equal,
    Plus,
    Star,
    Semicolon,
    Num,
    Ident,
    Var,
    Eof
}

public class Scanner(string source)
{
    public List<Token> Scan()
    {
        var tokens = new List<Token>();

        // TODO: parse tokens
        
        return tokens;
    }
}
