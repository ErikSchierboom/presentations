# .NET 10

## .NET libraries

- `CompareOptions.NumericOrdering` (`"2"` < `"10"`)
- ZipArchive lezen/bijwerken is sneller
- Async ZIP API's
- Nieuwe cryptografie utility methoden

## .NET Runtime

- [Performance improvements in .NET 10](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-10/)

## .NET SDK

- dotnet tool uitvoeren zonder installatie: `dotnet tool exec <tool-package>` (of `dnx <tool-package>`)
- `Microsoft.Testing.Platform` geintroduceerd als vervanger van `vstest`
  - Voordeel: snelheid
  - Uitbreidbaar met extensions (via NuGet packages)
- Nieuwe solution files (`dotnet sln migrate`)
- File-based apps
  - `dotnet run <file.cs>` of `dotnet <file.cs>`; geen `.csproj` file nodig
  - Ideaal voor kleine tests
  - Gebruik `args` variabele
  - Package refenties (syntax op NuGet, bv. https://www.nuget.org/packages/humanizer/)
  - Project referentie
  - SDK referentie
  - BuildInfo
    - Performance (virtueel project)
  - `dotnet publish` (native AOT)
  - `dotnet project convert <file.cs>`

## C# 14

- Extension members
- Null-conditional assignment
- `nameof` supports unbound generic types
- More implicit conversions for `Span<T>` and `ReadOnlySpan<T>`
- Modifiers on simple lambda parameters
- Field backed properties
- Partial events and constructors
- User-defined compound assignment operators
