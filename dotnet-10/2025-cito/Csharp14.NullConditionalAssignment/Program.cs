Customer? customer = null;
if (customer is not null)
{
    customer.Name = "Bob";   
}

Console.WriteLine(customer?.Name);

class Customer
{
    public string Name { get; set; }
};