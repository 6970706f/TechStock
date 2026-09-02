using TechStock.Domain.Entities;

namespace TechStock.Infrastructure.Repositories;

public class UserRepository
{
    private readonly ICollection<User> Users = [];

    public void Add(User user)
        => Users.Add(user);

    public void Delete(User user)
        => Users.Remove(user);

    public ICollection<User> GetAll()
        => Users.ToList();
    
    public User? GetById(int id)
        => Users.FirstOrDefault(user => user.Id == id);
}
