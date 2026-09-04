using ErrorOr;
using TechStock.Application.DTOs;
using TechStock.Application.Validators;
using TechStock.Domain.Entities;
using TechStock.Domain.Enums;
using TechStock.Infrastructure.Repositories;

namespace TechStock.Application.Services;

public class ProductService(
    ProductRepository repository,
    ProductValidators productValidators
)
{
    public ErrorOr<Created> Add(ProductCreateRequest request)
    {
        return productValidators.AddValidator(request)
            .Then(_ => GetStoreErrorOr())
            .Then(store =>
            {
                var product = new Product(
                    request.Name,
                    request.Price,
                    request.Quantity,
                    store
                );

                store.AddProduct(product);
                repository.Add(product);

                return Result.Created;
            });
    }

    public ErrorOr<Deleted> Delete(Guid id)
    {
        var store = GetStoreErrorOr();
        if (store.IsError)
            return store.Errors;

        var product = GetProductByIdErrorOr(id);
        if (product.IsError)
            return product.Errors;

        repository.Delete(product.Value);
        store.Value.RemoveProduct(product.Value);

        return Result.Deleted;
    }

    public ErrorOr<Updated> Update(Guid id, ProductUpdateRequest request)
    {
        var store = GetStoreErrorOr();
        if (store.IsError)
            return store.Errors;

        var product = GetProductByIdErrorOr(id);
        if (product.IsError)
            return product.Errors;

        var validation = productValidators.UpdateValidator(store.Value, product.Value, request);
        if (validation.IsError)
            return validation.Errors;

        if (request.Name != product.Value.Name)
            product.Value.ChangeName(request.Name);
        
        if (request.Price != product.Value.Price)
            product.Value.ChangePrice(request.Price);
        
        if (request.Quantity != product.Value.Quantity)
            product.Value.ChangeQuantity(request.Quantity);
        
        return Result.Updated;
    }

    public ErrorOr<ProductResponse> GetById(Guid id)
    {
        var product = GetProductByIdErrorOr(id);
        if (product.IsError)
            return product.Errors;

        return ToDTO(product.Value);
    }

    public ErrorOr<IEnumerable<ProductResponse>> GetAllPerStore()
    {
        var store = GetStoreErrorOr();
        if (store.IsError)
            return store.Errors;

        var products = repository.GetAllPerStore(store.Value);

        return products.Select(ToDTO).ToList();
    }

    public ErrorOr<Updated> MovementStock(Guid id, ProductMovementRequest request)
    {
        var store = GetStoreErrorOr();
        if (store.IsError)
            return store.Errors;

        var product = GetProductByIdErrorOr(id);
        if (product.IsError)
            return product.Errors;
        
        var validation = productValidators.MovementStockValidator(product.Value, request);
        if (validation.IsError)
            return validation.Errors;

        if (request.Type == ProductMovementType.Entry)
            product.Value.AddStock(request.Quantity);
        
        if (request.Type == ProductMovementType.Exit)
            product.Value.RemoveStock(request.Quantity);
        
        return Result.Updated;
    }

    private ErrorOr<Product> GetProductByIdErrorOr(Guid id)
    {
        var product = repository.GetById(id);
        if (product is null)
            return Error.NotFound(
                code: "Product.NotFound",
                description: "product not found"
            );

        return product;
    }

    private ErrorOr<Store> GetStoreErrorOr()
    {
        var user = LoggedUser.Get();
        if (user is null)
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "user unauthorized"
            );
        
        return user.Store;
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
