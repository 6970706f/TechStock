namespace TechStock.Domain.Entities;

public class Store
{
    public Store(int id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();
        
        Id = id;
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }

    public ICollection<User> Users { get; private set; } = [];
    public ICollection<Product> Products { get; private set; } = [];

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();
        
        Name = name;
    }
}
