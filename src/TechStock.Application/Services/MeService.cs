using TechStock.Application.DTOs;
using TechStock.Domain.Entities;
using TechStock.Infrastructure.Repositories;

namespace TechStock.Application.Services;

public class UserService(
    UserRepository repository
)
{
    public void Delete()
    {
        repository.Delete(GetLoggedUserOrThrow());
    }

    public UserResponse GetLoggedUser()
    {
        return ToDTO(GetLoggedUserOrThrow());
    }

    public void ChangeName(UserChangeNameRequest request)
    {
        var user = GetLoggedUserOrThrow();

        user.ChangeName(request.Name);
    }

    public void ChangePassword(UserChangePasswordRequest request)
    {
        var user = GetLoggedUserOrThrow();

        if (request.OldPassword != user.PasswordHash || request.ConfirmPassword != request.NewPassword)
            throw new Exception();

        user.ChangePassword(request.NewPassword);
    }

    private User GetLoggedUserOrThrow()
    {
        User user = LoggedUser.Current
            ?? throw new Exception();
        
        return user;
    }

    private UserResponse ToDTO(User user)
    {
        return new UserResponse(
            user.Id,
            user.Name
        );
    }
}
