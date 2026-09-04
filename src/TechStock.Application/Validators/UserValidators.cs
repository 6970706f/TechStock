using ErrorOr;
using TechStock.Application.DTOs;
using TechStock.Domain.Entities;
using TechStock.Infrastructure.Services;

namespace TechStock.Application.Validators;

public class UserValidators(
    PasswordService passwordService
)
{
    public ErrorOr<Updated> ChangeNameValidator(UserChangeNameRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Error.Validation(
                code: "User.NameValidation",
                description: "invalid name"
            );

        return Result.Updated;
    }

    public ErrorOr<Updated> ChangePasswordValidator(User user, UserChangePasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.OldPassword))
            return Error.Validation(
                code: "User.InvalidCredentials",
                description: "invalid credentials"
            );

        if (!passwordService.VerifyPassword(request.OldPassword, user.PasswordHash) || 
        request.NewPassword != request.ConfirmPassword)
            return Error.Validation(
                code: "User.InvalidCredentials",
                description: "invalid credentials"
            );
        
        return Result.Updated;
    }
}
