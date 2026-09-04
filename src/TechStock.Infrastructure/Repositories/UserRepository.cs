using TechStock.Domain.Entities;

namespace TechStock.Infrastructure.Repositories;

public class UserRepository
{
    private readonly ICollection<User> Users = [];

    public void Add(User user)
        => Users.Add(user);

    public void Delete(User user)
        => Users.Remove(user);
    
    public User? GetByName(string name)
        => Users.FirstOrDefault(user => user.Name == name);
    
    public bool ExistsByName(string name)
        => Users.Any(user => user.Name == name);
}
