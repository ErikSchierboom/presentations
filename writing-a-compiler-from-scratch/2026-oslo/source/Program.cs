const string code = """
                    var x = 3;
                    var result = 1 + 2 * x;
                    result + 10
                    """;

var tokens = new Scanner(code).Scan();
foreach (var token in tokens)
{
    Console.WriteLine(token);
}

public class Scanner(string source)
{
    public List<Token> Scan()
    {
        throw new NotImplementedException();
    }
}

public enum TokenType
{
    Equal,
    Plus,
    Star,
    Semicolon,
    Num,
    Ident,
    Var,
}

public record Token(TokenType Type, string Lexeme);
