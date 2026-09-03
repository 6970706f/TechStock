namespace TechStock.Domain.Entities;

public class Product
{
    public Product(int id, string name, decimal price, int quantity, Store store)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();
        if (price <= 0)
            throw new Exception();
        
        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;

        StoreId = store.Id;
        Store = store;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    public int StoreId { get; private set; }
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

    public void ChangeQuantity(int quantity)
    {
        if (quantity < 0)
            throw new Exception();
        
        Quantity = quantity;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new Exception();
        
        if (Quantity < quantity)
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
