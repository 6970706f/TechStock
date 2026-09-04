using ErrorOr;
using TechStock.Application.DTOs;

namespace TechStock.Application.Validators;

public class StoreValidator
{
    public ErrorOr<Success> AddValidator(
        StoreCreateRequest storeRequest,
        UserCreateRequest userRequest
    )
    {
        if (string.IsNullOrWhiteSpace(userRequest.Name))
            return Error.Validation(
                code: "User.NameValidation",
                description: "invalid credentials"
            );
        
        return Result.Success;
    }

    public ErrorOr<Success> ChangeNameValidator(StoreUpdateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Error.Validation(
                code: "Store.NameValidation",
                description: "invalid name"
            );
        
        return Result.Success;
    }
}
