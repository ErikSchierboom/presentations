var customer = new Customer();
customer.Name = null;

Console.WriteLine(customer.Name);

class Customer
{
    public string Name
    {
        get;
        set => field = value ?? throw new ArgumentNullException(nameof(value));
    }
}