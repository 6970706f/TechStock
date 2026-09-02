using TechStock.Domain.Entities;

namespace TechStock.Infrastructure.Repositories;

public class UserRepository
{
    private readonly ICollection<User> Users = [];

    public void Add(User user)
        => Users.Add(user);

    public void Delete(User user)
        => Users.Remove(user);
}
