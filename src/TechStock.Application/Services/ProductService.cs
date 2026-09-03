using ErrorOr;
using TechStock.Application.DTOs;
using TechStock.Domain.Entities;
using TechStock.Domain.Enums;
using TechStock.Infrastructure.Repositories;

namespace TechStock.Application.Services;

public class ProductService(
    ProductRepository repository
)
{
    public ErrorOr<Created> Add(ProductCreateRequest request)
    {
        int id = Random.Shared.Next();
        var store = GetStoreErrorOr();
        var validateProduct = ValidateProduct(request.Name, request.Price, request.Quantity);

        if (store.IsError)
            return store.Errors;
        
        if (validateProduct.IsError)
            return validateProduct.Errors;

        var product = new Product(
            id,
            request.Name,
            request.Price,
            request.Quantity,
            store.Value
        );

        store.Value.AddProduct(product);
        repository.Add(product);

        return Result.Created;
    }

    public ErrorOr<Deleted> Delete(int id)
    {
        var product = GetProductByIdErrorOr(id);
        var store = GetStoreErrorOr();

        if (store.IsError)
            return store.Errors;

        if (product.IsError)
            return product.Errors;

        repository.Delete(product.Value);
        store.Value.RemoveProduct(product.Value);

        return Result.Deleted;
    }

    public ErrorOr<Updated> Update(int id, ProductUpdateRequest request)
    {
        var product = GetProductByIdErrorOr(id);
        var validateProduct = ValidateProduct(request.Name, request.Price, request.Quantity);

        if (product.IsError)
            return product.Errors;

        if (validateProduct.IsError)
            return product.Errors;

        if (request.Name != product.Value.Name)
            product.Value.ChangeName(request.Name);
        
        if (request.Price != product.Value.Price)
            product.Value.ChangePrice(request.Price);
        
        if (request.Quantity != product.Value.Quantity)
            product.Value.ChangeQuantity(request.Quantity);
        
        return Result.Updated;
    }

    public ErrorOr<ProductResponse> GetById(int id)
    {
        var product = GetProductByIdErrorOr(id);

        if (product.IsError)
            return product.Errors;

        return ToDTO(product.Value);
    }

    public IEnumerable<ProductResponse> GetAll()
    {
        var products = repository.GetAll();

        return products.Select(ToDTO);
    }

    public ErrorOr<Updated> MovementStock(int id, ProductMovementRequest request)
    {
        var product = GetProductByIdErrorOr(id);

        if (product.IsError)
            return product.Errors;

        if (request.Quantity < 0)
            return Error.Validation(
                code: "Product.QuantityValidation",
                description: "quantity cannot be negative"
            );
        
        if (request.Quantity > product.Value.Quantity && request.Type == ProductMovementType.Exit)
            return Error.Conflict(
                code: "Product.QuantityConflict",
                description: "insufficient product quantity in stock"
            );

        if (request.Type == ProductMovementType.Entry)
            product.Value.AddStock(request.Quantity);
        
        if (request.Type == ProductMovementType.Exit)
            product.Value.RemoveStock(request.Quantity);
        
        return Result.Updated;
    }

    private ErrorOr<Product> GetProductByIdErrorOr(int id)
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

    private ErrorOr<Success> ValidateProduct(string name, decimal price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation(
                code: "Product.NameValidation",
                description: "invalid name"
            );

        if (price <= 0)
            return Error.Validation(
                code: "Product.PriceValidation",
                description: "price must be greater than 0"
            );
        
        if (quantity < 0)
            return Error.Validation(
                code: "Product.QuantityValidation",
                description: "quantity cannot be negative"
            );
        
        return Result.Success;
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
