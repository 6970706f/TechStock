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
    UserValidator userValidators,
    LoginService loginService
)
{
    public ErrorOr<Deleted> Delete()
    {
        return GetLoggedUserErrorOr()
            .Then(user =>
            {
                repository.Delete(user);
                loginService.Logout();

                return Result.Deleted;
            });
    }

    public ErrorOr<UserResponse> GetMe()
    {
        return GetLoggedUserErrorOr()
            .Then(user =>
            {
                return ToDTO(user);
            });
    }

    public ErrorOr<Updated> ChangeName(UserChangeNameRequest request)
    {
        return GetLoggedUserErrorOr()
            .Then(user => userValidators.ChangeNameValidator(request)
            .Then<Updated>(_ =>
            {
                if (repository.ExistsByName(request.Name))
                    return Error.Conflict(
                        code: "User.NameConflict",
                        description: "user with this name already exists"
                    );

                user.ChangeName(request.Name);

                return Result.Updated;
            }));
    }

    public ErrorOr<Updated> ChangePassword(UserChangePasswordRequest request)
    {
        return GetLoggedUserErrorOr()
            .Then(user => userValidators.ChangePasswordValidator(user, request)
            .Then(_ =>
            {
                user.ChangePassword(passwordService.HashPassword(request.NewPassword));

                return Result.Updated;
            }));
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
