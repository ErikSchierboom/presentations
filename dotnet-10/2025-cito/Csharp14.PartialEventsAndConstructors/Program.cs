Console.WriteLine(new PartialConstructor().ToString());

partial class PartialConstructor
{
    public partial PartialConstructor();
}

partial class PartialConstructor
{
    public partial PartialConstructor()
    {
        Console.WriteLine("Partial constructor");
    }
}
