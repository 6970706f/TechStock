using ErrorOr;
using TechStock.Application.DTOs;
using TechStock.Domain.Enums;

namespace TechStock.Application.Validators;

public class ProductValidator
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
                description: "quantity must be greater than 0"
            );
        
        return Result.Success;
    }

    public ErrorOr<Success> UpdateValidator(ProductUpdateRequest request)
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
        
        return Result.Success;
    }

    public ErrorOr<Success> MovementStockValidator(ProductMovementRequest request)
    {
        if (request.Type != ProductMovementType.Entry ||
            request.Type != ProductMovementType.Exit)
        {
            return Error.Validation(
                code: "Product.TypeValidation",
                description: "invalid type"
            );
        }

        if (request.Quantity <= 0)
            return Error.Validation(
                code: "Product.QuantityValidation",
                description: "quantity must be greater than 0"
            );

        return Result.Success;
    }
}
