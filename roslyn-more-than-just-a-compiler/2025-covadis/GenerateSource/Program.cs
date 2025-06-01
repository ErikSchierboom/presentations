// Roslyn Quoter: https://roslynquoter.azurewebsites.net/

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

var code = CompilationUnit()
    .AddMembers(
        RecordDeclaration(Token(SyntaxKind.RecordKeyword), Identifier("Movie"))
            .AddParameterListParameters(
                Parameter(Identifier("Title"))
                    .WithType(PredefinedType(Token(SyntaxKind.StringKeyword))),
                Parameter(Identifier("Year"))
                    .WithType(PredefinedType(Token(SyntaxKind.IntKeyword)))
                )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)))
    .NormalizeWhitespace();
   
var generateSourceFilePath = Path.GetFullPath(Path.Combine("..", "..", "..", "Movie.cs"));
File.WriteAllText(generateSourceFilePath, code.ToFullString());
