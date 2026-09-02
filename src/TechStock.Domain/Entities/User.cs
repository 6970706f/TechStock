using TechStock.Domain.Enums;

namespace TechStock.Domain.Entities;

public class User
{
    public User(int id, string name, string passwordHash, Store store, Role? role)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception();
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new Exception();
        
        Id = id;
        Name = name;
        PasswordHash = passwordHash;
        if (role != null)
            Role = (Role)role;

        IdStore = store.Id;
        Store = store;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; } = Role.User;

    public int IdStore { get; private set; }
    public Store Store { get; private set; } = null!;

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

    public void ChangeRole(Role role)
    {
        Role = role;
    }
}
