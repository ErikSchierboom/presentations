var str = "Hello World!";
Console.WriteLine(str.Words());

// var numbers = new[] { 1, 2, 3 };
// Console.WriteLine(numbers.FirstOne());
// Console.WriteLine(numbers.FirstOne2);
//
// Console.WriteLine(IEnumerable<int>.MyEmpty());
// Console.WriteLine(IEnumerable<int>.MyEmpty2);
// Console.WriteLine(numbers + numbers);

static class EnumerableExtensions
{
    public static string[] Words(this string text) => text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    
    // TODO: convert to extension block
    // TODO: add extension property
    // TODO: add extension block for static extension method
    // TODO: add extension block for static extension property
    // TODO: add extension block for static extension operator
}
