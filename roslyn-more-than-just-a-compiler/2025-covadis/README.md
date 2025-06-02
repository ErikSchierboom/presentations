# Roslyn: Meer Dan Alleen een Compiler

## Introductie (5 min.)

- Roslyn is de code naam voor het .NET Compiler Platform, waar zowel de compilers als code analyse API's voor C# én VB toe behoren
- Oorspronkelijke C# compiler was geschreven in C++
- Doel van Roslyn:
  - Eén enkele plaats om C# code te parsen
  - Open voor gebruik externe partijen (en open-source)
  - Verbeteren onderhoudbaarheid en verhoogde productiviteit (sneller nieuwe features)
  - Dogfooding (gebruik C# in de C# compiler)

## Opzet compiler

Fases:

1. Parse fase
   - Text omzetten naar een Abstract Syntax Tree (AST)
   - Syntax nodes, tokens en trivia
     - Trivia zijn comments, whitespace en andere niet-code elementen
   - Syntax tree is immutable
2. Declaration fase
   - Identificeert symbolen (zoals classes, methods, etc.) in de syntax tree
   - Maakt een symbol table aan
   - Verzamelt informatie over de symbolen (zowel eigen symbolen als uit referenties)
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

## Demo 1: scripting (3 min.)

Doel: demonstreren van Roslyn scripting API's

Demo:

- Voeg `Microsoft.CodeAnalysis.CSharp.Scripting` package toe
- Gebruik van `CSharpScript.RunAsync` om C# code uit te voeren
- Print `state.ReturnValue`
- Vermeld `state.ContinueWithAsync`

## Demo 2: analyseer code (20 min.)

Doel: introductie van Roslyn's API voor het werken met C# code

Demo:

- Toevoegen van `Microsoft.CodeAnalysis.CSharp` package
- Gebruik van `CSharpSyntaxTree.ParseText` om C# code te parsen
- Laat Abstract Syntax Tree (AST) zien mbv. Syntax Visualizer en https://sharplab.io/
- Verschil tussen `SyntaxNode`, `SyntaxToken` en `SyntaxTrivia`
  1. Gebruik file-scoped namespace
     - `syntaxTree.GetRoot()`
     - `root.DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>()`
  2. Gebruik exponent notatie
     - `methodDeclaration.DescendantTokens()`
     - `token.IsKind(SyntaxKind.NumericLiteralToken)`
     - `token.Value is 1e9`
     - `token.Text != "1e9"`
  3. Gebruik `AddSeconds`
  - `var compilation = CSharpCompilation.Create("Analyzing")`
  - `syntaxTrees: [syntaxTree]`
  - `references: [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)]`
  - `options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)`
  - `var diagnostics = compilation.GetDiagnostics();`
  - `var semanticModel = compilation.GetSemanticModel(syntaxTree);`
  - `var dateTimeStruct = compilation.GetSpecialType(SpecialType.System_DateTime);`
  - `var addSecondsCall = dateTimeStruct.GetMembers("AddSeconds").Single();`
  - `var invocationExpression = root.DescendantNodes().OfType<InvocationExpressionSyntax>() .Single();`
  - `var operation = (IInvocationOperation)semanticModel.GetOperation(invocationExpression)!;`
  - `var doesNotUseAddSeconds = !operation.TargetMethod.Equals(addSecondsCall, SymbolEqualityComparer.Default);`

## Demo 3: herschrijven code (10 min.)

Doel: demonstreren van Roslyn's API voor het herschrijven van C# code

- Gebruik van `SyntaxFactory` om nieuwe nodes te maken

Demo:

- Wegschrijven tree met: `File.WriteAllText(sourceFilePath, root.ToFullString());`
- Normaliseren whitespace met `.NormalizeWhitespace()`
- Immutable tree `root.RemoveNodes(root.DescendantNodes().OfType<EmptyStatementSyntax>(), SyntaxRemoveOptions.AddElasticMarker)`
- Gebruik van `SyntaxRewriter` om nodes te herschrijven
  1. Verwijder empty statements
     - `VisitEmptyStatement`
  2. Gebruik exponent notatie
     - `VisitToken`
     - `SyntaxFactory.Literal("1e9", 1e9)`

## Demo 4: solution-wide herschrijven (8 min.)

Doel: demonstreren van Roslyn's API voor het herschrijven van code in een solution

Demo:

- Gebruik van `MSBuildWorkspace` om een project te laden
- `compilation.GetTypeByMetadataName("Xunit.FactAttribute")` om een type te vinden
- Gebruik van `DocumentEditor` om eenvoudiger wijzigingen aan te brengen in een document
- `documentEditor.SemanticModel.GetDeclaredSymbol(methodDeclarationSyntax)`
- Gebruik `Renamer.RenameSymbolAsync` om symbolen te hernoemen
- Hoe te werken met immutable solution, projecten en documenten

## Demo 5: analyzer

Doel: laat zien wat analyzers zijn en hoe je ze kunt gebruiken

Demo:

1. Exponent notatie analyzer
   - `context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.NumericLiteralExpression)`
   - `var diagnostic = Diagnostic.Create(Rule, literalExpression.GetLocation(), literalExpression.Token.Text);`
   - `context.ReportDiagnostic(diagnostic);`
   - Laat tests zien
2. Exponent notatie fixer
   - `var codeAction = CodeAction.Create(`
   - `createChangedSolution: ct => UseExponentNotation(context.Document, diagnostic.Location.SourceSpan, ct)`
   - `getInnermostNodeForTie: true`
   - `var newLiteralExpression = literalExpression.WithToken(SyntaxFactory.Literal("1e9", 1e9));`

## Demo 6: genereer code

Doel: demonstreren van Roslyn code generatie API's

Demo:

- Maak record aan voor [`Actor`, `Movie`, `Director`]
- Demonstreer gebruik `using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;`
- Gebruik van Roslyn Quoter website (https://roslynquoter.azurewebsites.net/)
- Laat zien dat als je een fout hebt in de gegenereerde code, dat je dan de code niet kunt compileren
- Gebruik `.NormalizeWhitespace()`
- `Token(SyntaxKind.PublicKeyword).WithLeadingTrivia(Comment("// <auto-generated>"))`
- `Token(SyntaxKind.PartialKeyword)`
- Toevoegen van `// <auto-generated>` commentaar om aan te geven dat de code automatisch is gegenereerd

## Demo 7: source generator

Doel: demonstreren van Roslyn source generator API's

Demo:

- `var files = context.AdditionalTextsProvider.Where(f => Path.GetFileName(f.Path) == "Models.txt").Collect();`
- `var code = RecordDeclaration(Token(SyntaxKind.RecordKeyword), className)`
  `.AddModifiers(`
  `Token(SyntaxKind.PublicKeyword).WithLeadingTrivia(Comment("// <auto-generated>")),`
  `Token(SyntaxKind.PartialKeyword))`
  `.WithSemicolonToken(Token(SyntaxKind.SemicolonToken))`
  `.NormalizeWhitespace();`
- `context.AddSource($"{className}.g.cs", code.ToFullString());`

## Demo 8: in-memory compilatie (3 min.)

Doel: laat zien hoe je code in-memory kunt compileren met Roslyn

Demo:

- `compilation.Emit(stream)`
- `var assembly = Assembly.Load(stream.ToArray())`
- `var movieType = assembly.GetType("Movie")`
- `Activator.CreateInstance(movieType, "Inception", 2010)`
