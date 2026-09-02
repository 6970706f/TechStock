using TechStock.Domain.Entities;

namespace TechStock.Infrastructure.Repositories;

public class StoreRepository(
    UserRepository userRepository
)
{
    private readonly ICollection<Store> Stores = [];

    public void Add(Store store)
        => Stores.Add(store);
    
    public void Delete(Store store)
    {
        foreach (var user in store.Users)
        {
            userRepository.Delete(user);
        }

        Stores.Remove(store);
    }
}
