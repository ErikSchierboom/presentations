Customer? customer = null;
customer?.Name = "Bob";

Console.WriteLine(customer?.Name);

class Customer
{
    public string Name { get; set; }
};