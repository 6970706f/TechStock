namespace TechStock.Domain.Entities;

public class Component : Product
{
    public Component(
        string name,
        decimal price,
        int quantity,
        Store store,
        string manufacturer
    ) : base(name, price, quantity, store)
    {
        if (string.IsNullOrWhiteSpace(manufacturer))
            throw new Exception();
        
        Manufacturer = manufacturer;
    }

    public string Manufacturer { get; private set; }

    public void ChangeManufacturer(string manufacturer)
    {
        if (string.IsNullOrWhiteSpace(manufacturer))
            throw new Exception();
        
        Manufacturer = manufacturer;
    }
}
