namespace BoomScript;

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
        var position = 0;

        while (position < source.Length)
        {
            var character = source[position];
            switch (character)
            {
                case ' ' or '\r' or '\n':
                    position++;
                    continue;
                case '+':
                    tokens.Add(new Token(TokenType.Plus, "+"));
                    position++;
                    break;
                case '*':
                    tokens.Add(new Token(TokenType.Star, "*"));
                    position++;
                    break;
                case ';':
                    tokens.Add(new Token(TokenType.Semicolon, ";"));
                    position++;
                    break;
                case '=':
                    tokens.Add(new Token(TokenType.Equal, "="));
                    position++;
                    break;
                case >= '0' and <= '9':
                    var integerStart = position;
                    
                    while (position < source.Length && source[position] is >= '0' and <= '9')
                        position++;
                    
                    tokens.Add(new Token(TokenType.Num, source[integerStart..position]));
                    break;
                case >= 'a' and <= 'z':
                    var identifierStart = position;
                    
                    while (position < source.Length && source[position] is >= 'a' and <= 'z')
                        position++;

                    var lexeme = source[identifierStart..position];
                    if (lexeme == "var")
                        tokens.Add(new Token(TokenType.Var, lexeme));
                    else
                        tokens.Add(new Token(TokenType.Ident, lexeme));
                    break;
                default:
                    throw new Exception($"Unexpected character: {character}.");
            }
        }
        
        tokens.Add(new Token(TokenType.Eof, ""));

        return tokens;
    }
}
