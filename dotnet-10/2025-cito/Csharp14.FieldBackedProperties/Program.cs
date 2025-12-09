var customer = new Customer();
customer.Name = null;

Console.WriteLine(customer.Name);

class Customer
{
    private string _name;
    
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }
}