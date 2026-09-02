using TechStock.Application.DTOs;
using TechStock.Domain.Entities;
using TechStock.Infrastructure.Repositories;

namespace TechStock.Application.Services;

public class ProductService(
    ProductRepository repository
)
{
    public void Add(ProductCreateRequest request)
    {
        int id = Random.Shared.Next();
        var store = LoggedUser.Get().Store;
        var product = new Product(
            id,
            request.Name,
            request.Price,
            request.Quantity,
            store
        );

        store.AddProduct(product);
        repository.Add(product);
    }

    public void Delete(int id)
    {
        var product = GetOrThrow(id);
        var store = LoggedUser.Get().Store;

        repository.Delete(product);
        store.RemoveProduct(product);
    }

    public void Update(int id, ProductUpdateRequest request)
    {
        var product = GetOrThrow(id);

        if (request.Name != product.Name)
            product.ChangeName(request.Name);
        
        if (request.Price != product.Price)
            product.ChangePrice(request.Price);
        
        if (request.Quantity != product.Quantity)
            product.ChangeQuantity(request.Quantity);
    }

    public ProductResponse GetById(int id)
    {
        var product = GetOrThrow(id);

        return ToDTO(product);
    }

    public IEnumerable<ProductResponse> GetAll()
    {
        var products = repository.GetAll();

        return products.Select(ToDTO);
    }

    private Product GetOrThrow(int id)
    {
        var product = repository.GetById(id)
            ?? throw new Exception();

        return product;
    }

    private ProductResponse ToDTO(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Price,
            product.Quantity
        );
    }
}
