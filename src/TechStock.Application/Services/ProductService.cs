using ErrorOr;
using TechStock.Application.DTOs;
using TechStock.Application.Validators;
using TechStock.Domain.Entities;
using TechStock.Domain.Enums;
using TechStock.Infrastructure.Repositories;

namespace TechStock.Application.Services;

public class ProductService(
    ProductRepository repository,
    ProductValidator productValidators
)
{
    public ErrorOr<Created> Add(ProductCreateRequest request)
    {
        return productValidators.AddValidator(request)
            .Then(_ => GetStoreErrorOr()
            .Then<Created>(store =>
            {
                if (repository.ExistsByName(request.Name))
                    return Error.Conflict(
                        code: "Product.NameConflict",
                        description: "already exists a product with this name"
                    );

                var product = new Product(
                    request.Name,
                    request.Price,
                    request.Quantity,
                    store
                );

                store.AddProduct(product);
                repository.Add(product);

                return Result.Created;
            }));
    }

    public ErrorOr<Deleted> Delete(Guid id)
    {
        return GetStoreErrorOr()
            .Then(store => GetProductErrorOr(id, store)
            .Then(product =>
            {
                repository.Delete(product);
                store.RemoveProduct(product);

                return Result.Deleted;
            }));
    }

    public ErrorOr<Updated> Update(Guid id, ProductUpdateRequest request)
    {
        return GetStoreErrorOr()
            .Then(store => GetProductErrorOr(id, store)
            .Then(product => productValidators.UpdateValidator(request)
            .Then<Updated>(_ =>
            {
                if (repository.ExistsByName(request.Name))
                    return Error.Conflict(
                        code: "Product.NameConflict",
                        description: "already exists a product with this name"
                    );

                if (request.Name != product.Name)
                    product.ChangeName(request.Name);
                
                if (request.Price != product.Price)
                    product.ChangePrice(request.Price);
                
                return Result.Updated;
            })));
    }

    public ErrorOr<ProductResponse> GetById(Guid id)
    {
        return GetStoreErrorOr()
            .Then(store => GetProductErrorOr(id, store)
            .Then(product =>
            {
                return ToDTO(product);
            }));
    }

    public ErrorOr<IEnumerable<ProductResponse>> GetAllPerStore()
    {
        return GetStoreErrorOr()
            .Then(store =>
            {
                var products = repository.GetAllPerStore(store);

                return products.Select(ToDTO);
            });
    }

    public ErrorOr<Updated> MovementStock(Guid id, ProductMovementRequest request)
    {
        return GetStoreErrorOr()
            .Then(store => GetProductErrorOr(id, store)
            .Then(product => productValidators.MovementStockValidator(request)
            .Then(_ =>
            {
                if (request.Type == ProductMovementType.Entry)
                    product.AddStock(request.Quantity);
                
                if (request.Type == ProductMovementType.Exit)
                    product.RemoveStock(request.Quantity);
                
                return Result.Updated;
            })));
    }

    private ErrorOr<Product> GetProductErrorOr(Guid id, Store store)
    {
        var product = repository.GetById(id, store);
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
