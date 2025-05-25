1. Introductie

- Roslyn is de code naam voor het .NET Compiler Platform, waar zowel de compilers als code analyse API's voor C# én VB toebehoren
- Doel:

  - Eén enkele plaats om C# code te parsen
  - Verbeteren onderhoudbaarheid
  - Dogfooding
  - Open (-source)

2. Opzet compiler

- Meerdere fases

1. Parse fase
2. Declaration fase
3. Bind fase
4. Emit fase

5. Parse fase

## Analyseren

## Herschrijven

- Curly braces en indentatie
- Gebruik van `var` waar mogelijk
- Namespace naar file-scoped namespace
- Mergen van twee constante strings


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
