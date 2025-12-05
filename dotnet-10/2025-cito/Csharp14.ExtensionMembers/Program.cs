var str = "Hello World!";
Console.WriteLine(str.Words());

var numbers = new[] { 1, 2, 3 };
Console.WriteLine(numbers.FirstOne());
Console.WriteLine(numbers.FirstOne2);

Console.WriteLine(IEnumerable<int>.MyEmpty());
Console.WriteLine(IEnumerable<int>.MyEmpty2);
Console.WriteLine(numbers + numbers);

static class EnumerableExtensions
{
    extension(string text)
    {
        public string[] Words() => text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }
    
    extension<T>(IEnumerable<T> source)
    {
        public T FirstOne() => source.First();
        public T FirstOne2 => source.First();
    }
    
    extension<T>(IEnumerable<T> source)
    {
        public static IEnumerable<T> MyEmpty() => Enumerable.Empty<T>();
        public static IEnumerable<T> MyEmpty2 => Enumerable.Empty<T>();
        public static IEnumerable<T> operator + (IEnumerable<T> first, IEnumerable<T> second) => first.Concat(second);
    }
}
