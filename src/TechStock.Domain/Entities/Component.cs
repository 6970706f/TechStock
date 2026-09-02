namespace TechStock.Domain.Entities;

public class Component : Product
{
    public Component(int id, string name, decimal price, string manufacturer)
        : base(id, name, price)
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
