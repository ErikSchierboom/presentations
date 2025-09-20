# Roslyn: Meer Dan Alleen een Compiler

## Introductie (5 min.)

- Roslyn is de code naam voor het .NET Compiler Platform
- Oorspronkelijke C# compiler was geschreven in C++
- Doel van Roslyn:
  - Eén enkele plaats om C# code te parsen
  - Verbeteren onderhoudbaarheid en verhoogde productiviteit (sneller nieuwe features)
  - Open voor gebruik externe partijen (en open-source)
  - Dogfooding (gebruik C# in de C# compiler)

## Opzet compiler

Fases:

1. Parse fase: parse broncode naar AST
2. Declaration fase: symbols en metadata
3. Bind fase: verbind symbolen aan identifiers
4. Emit fase: genereer IL code

Diagram: https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/compiler-api-model#compiler-pipeline-functional-areas

## Demo 1: analyseer code (10 min.)

Doel: introductie van Roslyn's API voor het werken met C# code

Demo:

- Installeer package: `Microsoft.CodeAnalysis.CSharp`
- Parse C# code: `CSharpSyntaxTree.ParseText`
- Toon AST: Syntax Visualizer en https://sharplab.io/
- Verschil tussen `SyntaxNode` en `SyntaxToken`
  1. Gebruik file-scoped namespace
  2. Gebruik exponent notatie

## Demo 2: herschrijven code (10 min.)

Doel: demonstreren van Roslyn's API voor het herschrijven van C# code

Demo:

- Wegschrijven tree: `root.ToFullString()`
- Normaliseren whitespace: `root.NormalizeWhitespace()`
- Immutable tree `root.RemoveNodes(root.DescendantNodes().OfType<EmptyStatementSyntax>(), SyntaxRemoveOptions.AddElasticMarker)`
- Gebruik van `SyntaxRewriter` om nodes te herschrijven
  1. Verwijder empty statements
  2. Gebruik exponent notatie
  - Gebruik `SyntaxFactory.Literal` voor nieuw token

## Demo 3: solution-wide herschrijven (10 min.)

Doel: demonstreren van Roslyn's API voor het herschrijven van code in een solution

Demo:

- Laad solution: `MSBuildWorkspace.Create()`
- Vind symbol voor extern package: `compilation.GetTypeByMetadataName("Xunit.FactAttribute")`
- Gebruik van `DocumentEditor`
- Symbol informatie voor methode: `SemanticModel.GetDeclaredSymbol(methodDeclarationSyntax)`
- Hernoem methode via `Renamer.RenameSymbolAsync`
- Laat zien dat solution/project/document immutable zijn

## Demo 4: analyzer (15 min.)

Doel: laat zien wat analyzers zijn en hoe je ze kunt gebruiken

Demo:

1. Exponent notatie analyzer
   - Bouw en laat resultaat in IDE zien
   - Laat tests zien
2. Exponent notatie fixer
   - Gebruik van: `getInnermostNodeForTie: true`
   - Bouw en laat resultaat in IDE zien
   - Laat tests zien
