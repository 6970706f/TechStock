namespace TechStock.Domain.Entities;

public class Store
{
    public Store(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();

        Name = name;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }

    public ICollection<User> Users { get; private set; } = [];
    public ICollection<Product> Products { get; private set; } = [];

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();
        
        Name = name;
    }

    public void AddUser(User user)
        => Users.Add(user);

    public void AddProduct(Product product)
        => Products.Add(product);
    
    public void RemoveProduct(Product product)
        => Products.Remove(product);
}
