using ErrorOr;
using TechStock.Application.DTOs;
using TechStock.Domain.Entities;
using TechStock.Domain.Enums;

namespace TechStock.Application.Validators;

public class ProductValidators
{
    public ErrorOr<Success> AddValidator(ProductCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Error.Validation(
                code: "Product.NameValidation",
                description: "invalid name"
            );

        if (request.Price <= 0)
            return Error.Validation(
                code: "Product.PriceValidation",
                description: "price must be greater than 0"
            );
        
        if (request.Quantity <= 0)
            return Error.Validation(
                code: "Product.QuantityValidation",
                description: "quantity cannot be negative"
            );
        
        return Result.Success;
    }

    public ErrorOr<Success> UpdateValidator(
        Store store,
        Product product,
        ProductUpdateRequest request
    )
    {
        if (store != product.Store)
            return Error.Unauthorized(
                code: "Product.Unauthorized",
                description: "product does not belong to the store"
            );

        if (string.IsNullOrWhiteSpace(request.Name))
            return Error.Validation(
                code: "Product.NameValidation",
                description: "invalid name"
            );

        if (request.Price <= 0)
            return Error.Validation(
                code: "Product.PriceValidation",
                description: "price must be greater than 0"
            );
        
        if (request.Quantity <= 0)
            return Error.Validation(
                code: "Product.QuantityValidation",
                description: "quantity cannot be negative"
            );
        
        return Result.Success;
    }

    public ErrorOr<Success> MovementStockValidator(Product product, ProductMovementRequest request)
    {
        if (request.Quantity < 0)
            return Error.Validation(
                code: "Product.QuantityValidation",
                description: "quantity cannot be negative"
            );
        
        if (request.Quantity > product.Quantity && request.Type == ProductMovementType.Exit)
            return Error.Conflict(
                code: "Product.QuantityConflict",
                description: "insufficient product quantity in stock"
            );
        
        return Result.Success;
    }
}
