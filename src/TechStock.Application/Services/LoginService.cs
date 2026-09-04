using ErrorOr;
using TechStock.Application.DTOs;
using TechStock.Domain.Entities;
using TechStock.Infrastructure.Repositories;
using TechStock.Infrastructure.Services;

namespace TechStock.Application.Services;

public class LoginService(
    PasswordService passwordService,
    UserRepository userRepository
)
{
    public ErrorOr<Success> Login(LoginRequest request)
    {
        var user = userRepository.GetByName(request.Name);

        if (user is null)
            return Error.NotFound(
                code: "User.NotFound",
                description: "user not found"
            );

        if (!passwordService.VerifyPassword(request.Password, user.PasswordHash))
            return Error.Failure(
                code: "Login.Failure",
                description: "invalid credentials"
            );
        
        LoggedUser.Login(user);

        return Result.Success;
    }

    public ErrorOr<User> GetLoggedUserErrorOr()
    {
        var user = LoggedUser.Get();

        if (user is null)
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "user unauthorized"
            );
        
        return user;
    }

    public void Logout()
        => LoggedUser.Logout();
}
