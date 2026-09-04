using ErrorOr;
using TechStock.Application.DTOs;

namespace TechStock.Application.Validators;

public class StoreValidators
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
        
        if (string.IsNullOrWhiteSpace(userRequest.Password) ||
        userRequest.ConfirmPassword != userRequest.Password)
            return Error.Validation(
                code: "User.Password",
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
