using ErrorOr;
using TechStock.Application.DTOs;
using TechStock.Application.Validators;
using TechStock.Domain.Entities;
using TechStock.Infrastructure.Repositories;
using TechStock.Infrastructure.Services;

namespace TechStock.Application.Services;

public class MeService(
    UserRepository repository,
    PasswordService passwordService,
    UserValidators userValidators
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
        
        var validation = userValidators.ChangeNameValidator(request);
        if (validation.IsError)
            return validation.Errors;

        user.Value.ChangeName(request.Name);

        return Result.Updated;
    }

    public ErrorOr<Updated> ChangePassword(UserChangePasswordRequest request)
    {
        var user = GetLoggedUserErrorOr();
        if (user.IsError)
            return user.Errors;
        
        var validation = userValidators.ChangePasswordValidator(user.Value, request);
        if (validation.IsError)
            return validation.Errors;

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
