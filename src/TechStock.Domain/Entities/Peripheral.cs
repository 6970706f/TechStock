namespace TechStock.Domain.Entities;

public class Peripheral : Product
{
    public Peripheral(int id, string name, decimal price, string connectionType)
        : base(id, name, price)
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
