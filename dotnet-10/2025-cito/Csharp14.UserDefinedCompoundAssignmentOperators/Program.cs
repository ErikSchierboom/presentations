var counter = new Counter();
counter.Add(3);

Console.WriteLine(counter.Count);

class Counter
{
    public int Count { get; set; }
    
    public void Add(int value)
    {
        Count += value;
    }
}