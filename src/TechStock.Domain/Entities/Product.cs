namespace TechStock.Domain.Entities;

public class Product
{
    public Product(string name, decimal price, int quantity, Store store)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();
        if (price <= 0)
            throw new Exception();
        if (quantity < 0)
            throw new Exception();

        Name = name;
        Price = price;
        Quantity = quantity;

        StoreId = store.Id;
        Store = store;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    public Guid StoreId { get; private set; }
    public Store Store { get; private set; }

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

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new Exception();
        
        if (Quantity <= quantity)
            throw new Exception();
        
        Quantity -= quantity;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new Exception();
        
        Quantity += quantity;
    }
}
