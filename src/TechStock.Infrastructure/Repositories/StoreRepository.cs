using TechStock.Domain.Entities;

namespace TechStock.Infrastructure.Repositories;

public class StoreRepository
{
    private readonly ICollection<Store> Stores = [];

    public void Add(Store store)
        => Stores.Add(store);
    
    public void Delete(Store store)
        => Stores.Remove(store);
}
