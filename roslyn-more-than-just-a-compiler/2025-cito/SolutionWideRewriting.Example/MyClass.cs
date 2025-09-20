namespace SolutionWideRewriting.Example;

public class MyClass
{
    public void Both_methods()
    {
        My_method();
        My_other_method();
    }

    private void My_method()
    {
        Console.WriteLine("My method");
    }

    private void My_other_method()
    {
        Console.WriteLine("My other method");
    }
}