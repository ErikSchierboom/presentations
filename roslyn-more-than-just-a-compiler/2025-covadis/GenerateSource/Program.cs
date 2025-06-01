// Roslyn Quoter: https://roslynquoter.azurewebsites.net/

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

var code = CompilationUnit()
    .AddMembers(
        ClassDeclaration("Greeter")
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddMembers(
                MethodDeclaration(
                        PredefinedType(Token(SyntaxKind.StringKeyword)),
                        Identifier("Greet"))
                    .AddModifiers(Token(SyntaxKind.PublicKeyword))
                    .AddBodyStatements(
                        ReturnStatement(
                            LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                Literal("Hello!"))))))
    .NormalizeWhitespace();
   
const string sourceFilePath = "/Users/erik/Code/presentations/roslyn-more-than-just-a-compiler/2025-covadis/GenerateSource/Greeter.cs";
File.WriteAllText(sourceFilePath, code.ToFullString());
