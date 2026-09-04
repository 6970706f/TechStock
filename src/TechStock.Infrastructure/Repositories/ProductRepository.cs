using TechStock.Domain.Entities;

namespace TechStock.Infrastructure.Repositories;

public class ProductRepository
{
    private readonly ICollection<Product> Products = [];

    public void Add(Product product)
        => Products.Add(product);
    
    public void Delete(Product product)
        => Products.Remove(product);

    public IEnumerable<Product> GetAllPerStore(Store store)
    {
        ICollection<Product> products = [];

        foreach (var product in Products)
        {
            if (product.Store == store)
                products.Add(product);
        }

        return products.ToList();
    }
    
    public Product? GetById(Guid id)
        => Products.FirstOrDefault(product => product.Id == id);
}
