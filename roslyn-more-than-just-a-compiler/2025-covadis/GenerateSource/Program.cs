// Roslyn Quoter: https://roslynquoter.azurewebsites.net/

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

var code = CompilationUnit()
    .AddMembers(
        RecordDeclaration(Token(SyntaxKind.RecordKeyword), Identifier("Movie"))
            .AddParameterListParameters(
                Parameter(Identifier("Title"))
                    .WithType(PredefinedType(Identifier("string"))),
                Parameter(Identifier("Year"))
                    .WithType(PredefinedType(Token(SyntaxKind.IntKeyword)))
                )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)))
    .NormalizeWhitespace();
   
const string sourceFilePath = "/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/GenerateSource/Movie.cs";
File.WriteAllText(sourceFilePath, code.ToFullString());
