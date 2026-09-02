namespace TechStock.Domain.Entities;

public class User
{
    public User(int id, string name, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new Exception();
        
        Id = id;
        Name = name;
        PasswordHash = passwordHash;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string PasswordHash { get; private set; }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();

        Name = name;
    }

    public void ChangePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new Exception();
        
        PasswordHash = passwordHash;
    }
}
