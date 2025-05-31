# Roslyn: More Than Just a Compiler

## Introductie

- Roslyn is de code naam voor het .NET Compiler Platform, waar zowel de compilers als code analyse API's voor C# én VB toe behoren
- Doel:

  - Eén enkele plaats om C# code te parsen
  - Verbeteren onderhoudbaarheid en verhoogde productiviteit (sneller nieuwe features)
  - Dogfooding
  - Open voor gebruik externe partijen (en open-source)

## Opzet compiler

Fases:

1. Parse fase: text omzetten naar een Abstract Syntax Tree (AST)
   - Syntax nodes, tokens en trivia
     - Trivia zijn comments, whitespace en andere niet-code elementen
   - Syntax tree is immutable
2. Declaration fase
   - Identificeert symbolen (zoals classes, methods, etc.) in de syntax tree
   - Maakt een symbol table aan
   - Verzamelt informatie over de symbolen
3. Bind fase
   - Verbindt symbolen met hun definitie
   - Controleert de semantiek van de code
   - Maakt een semantisch model aan
   - Voert type checking uit
   - Verzamelt informatie over types, methoden, etc.
4. Emit fase
   - Genereert de bytecode of IL (Intermediate Language) van de code
   - Dit is de code die uiteindelijk wordt uitgevoerd door de .NET runtime

Diagram: https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/compiler-api-model#compiler-pipeline-functional-areas

## Analyseren

Uitleg:

- Parse tekst naar een Abstract Syntax Tree (AST) mbv. CSharpSyntaxTree.ParseText
- Gebruik de SyntaxNode om de structuur van de code te analyseren
- Gebruik van MSBuildWorkspace om een project te laden

Taken:

1. Gebruik reguliere namespace
2. Maak class public
3. Gebruikt overloading
4. Gebruikt `null` als default waarde
5. Gebruikt string concatenatie
6. Gebruikt block method

## Herschrijven

Uitleg:

- Gebruik van CSharpSyntaxRewriter om de syntax tree te herschrijven
- Gebruik van SyntaxFactory om nieuwe nodes te maken

Taken:

1. Formatteer code
2. Gebruik file-scoped namespace
3. Gebruik `var` in plaats van expliciete types
4. Versimpel boolean expressies
5. Gebruik {} in if/else statements
6. Normaliseer integer literals
7. Hernoemen variabelen?
8. Ongebruikte variabelen?

## TODO

- Text naar Abstract Syntax Tree (AST)
- Immutable

- Compiler phases: parse (AST), declaration, bind en emit
- Roslyn immutable
- Syntax node vs syntax token vs syntax trivia
- Symbols + compilation
- Byte code + emit
- Syntax/operation visitor
- Semantic model
- Workspace
  - Renamer
  - SymbolFinder

Achtergrond: je bent net aangenomen en je verkent voor het eerst de codebase. Al snel zie je code die je niet zo mooi vindt, dus je past de code aan en gaat verder. De volgende file heeft dezelfde problemen, maar ook nog meer! Het blijkt dat de code base een zootje is. Het is aan jou om te kijken hoe dit te fixen.
