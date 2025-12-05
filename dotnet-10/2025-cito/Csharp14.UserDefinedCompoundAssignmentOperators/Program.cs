var counter = new Counter();
counter += 3;

Console.WriteLine(counter.Count);

class Counter
{
    public int Count { get; set; }
    
    public void operator +=(int value)
    {
        Count += value;
    }
}