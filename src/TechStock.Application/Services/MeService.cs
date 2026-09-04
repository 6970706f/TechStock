using ErrorOr;
using TechStock.Application.DTOs;
using TechStock.Domain.Entities;
using TechStock.Infrastructure.Repositories;
using TechStock.Infrastructure.Services;

namespace TechStock.Application.Services;

public class MeService(
    UserRepository repository,
    PasswordService passwordService
)
{
    public ErrorOr<Deleted> Delete()
    {
        var user = GetLoggedUserErrorOr();

        if (user.IsError)
            return user.Errors;
        
        repository.Delete(user.Value);
        LoggedUser.Logout();

        return Result.Deleted;
    }

    public ErrorOr<UserResponse> GetMe()
    {
        var user = GetLoggedUserErrorOr();

        if (user.IsError)
            return user.Errors;

        return ToDTO(user.Value);
    }

    public ErrorOr<Updated> ChangeName(UserChangeNameRequest request)
    {
        var user = GetLoggedUserErrorOr();

        if (user.IsError)
            return user.Errors;
        
        if (string.IsNullOrWhiteSpace(request.Name))
            return Error.Validation(
                code: "User.NameValidation",
                description: "invalid name"
            );

        user.Value.ChangeName(request.Name);

        return Result.Updated;
    }

    public ErrorOr<Updated> ChangePassword(UserChangePasswordRequest request)
    {
        var user = GetLoggedUserErrorOr();

        if (user.IsError)
            return user.Errors;
        
        if (string.IsNullOrWhiteSpace(request.OldPassword))
            return Error.Validation(
                code: "User.InvalidCredentials",
                description: "invalid credentials"
            );

        if (!passwordService.VerifyPassword(request.OldPassword, user.Value.PasswordHash) || 
        request.NewPassword != request.ConfirmPassword)
            return Error.Validation(
                code: "User.InvalidCredentials",
                description: "invalid credentials"
            );

        user.Value.ChangePassword(passwordService.HashPassword(request.NewPassword));

        return Result.Updated;
    }

    private UserResponse ToDTO(User user)
    {
        return new UserResponse(
            user.Id,
            user.Name
        );
    }

    private ErrorOr<User> GetLoggedUserErrorOr()
    {
        var user = LoggedUser.Get();

        if (user is null)
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "user unauthorized"
            );
        
        return user;
    }
}
