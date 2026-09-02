using TechStock.Application.DTOs;
using TechStock.Domain.Entities;
using TechStock.Infrastructure.Repositories;

namespace TechStock.Application.Services;

public class MeService(
    UserRepository repository
)
{
    public void Delete()
    {
        repository.Delete(LoggedUser.Get());
    }

    public UserResponse GetMe()
    {
        return ToDTO(LoggedUser.Get());
    }

    public void ChangeName(UserChangeNameRequest request)
    {
        var user = LoggedUser.Get();

        user.ChangeName(request.Name);
    }

    public void ChangePassword(UserChangePasswordRequest request)
    {
        var user = LoggedUser.Get();

        if (request.OldPassword != user.PasswordHash || request.NewPassword != request.ConfirmPassword)
            throw new Exception();

        user.ChangePassword(request.NewPassword);
    }

    private UserResponse ToDTO(User user)
    {
        return new UserResponse(
            user.Id,
            user.Name
        );
    }
}
