namespace TechStock.Domain.Entities;

public class Product
{
    public Product(int id, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();
        if (price <= 0)
            throw new Exception();
        
        Id = id;
        Name = name;
        Price = price;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();
        
        Name = name;
    }

    public void ChangePrice(decimal price)
    {
        if (price <= 0)
            throw new Exception();
        
        Price = price;
    }
}
