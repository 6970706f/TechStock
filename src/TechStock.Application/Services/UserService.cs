using TechStock.Application.DTOs;
using TechStock.Domain.Entities;
using TechStock.Infrastructure.Repositories;

namespace TechStock.Application.Services;

public class UserService(
    UserRepository repository
)
{
    public void Add(UserRequest request)
    {
        int id = Random.Shared.Next();
        var user = new User(id, request.Name, request.Password);

        repository.Add(user);
    }

    public void Delete()
    {
        repository.Delete(GetLoggedUserOrThrow());
    }

    public UserResponse GetLoggedUser()
    {
        return ToDTO(GetLoggedUserOrThrow());
    }

    public void ChangeName(string name)
    {
        var user = GetLoggedUserOrThrow();

        user.ChangeName(name);
    }

    public void ChangePassword(string password)
    {
        var user = GetLoggedUserOrThrow();

        user.ChangePassword(password);
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
