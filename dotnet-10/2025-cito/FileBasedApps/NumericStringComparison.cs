using System.Globalization;

Console.WriteLine(string.Compare("2", "10", CultureInfo.InvariantCulture, CompareOptions.None));
Console.WriteLine(string.Compare("2", "10", CultureInfo.InvariantCulture, CompareOptions.NumericOrdering));
