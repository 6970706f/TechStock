namespace TechStock.Domain.Entities;

public class Peripheral : Product
{
    public Peripheral(
        string name,
        decimal price,
        int quantity,
        Store store,
        string connectionType
    ) : base(name, price, quantity, store)
    {
        if (string.IsNullOrWhiteSpace(connectionType))
            throw new Exception();
        
        ConnectionType = connectionType;
    }

    public string ConnectionType { get; private set; }

    public void ChangeConnectionType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new Exception();
        
        ConnectionType = type;
    }
}
