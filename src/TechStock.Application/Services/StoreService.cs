using TechStock.Application.DTOs;
using TechStock.Domain.Entities;
using TechStock.Domain.Enums;
using TechStock.Infrastructure.Repositories;

namespace TechStock.Application.Services;

public class StoreService(
    StoreRepository storeRepository,
    UserRepository userRepository
)
{
    public void Add(StoreCreateRequest storeRequest, UserCreateRequest userRequest)
    {
        var store = new Store(Random.Shared.Next(), storeRequest.Name);

        var user = new User(
            Random.Shared.Next(),
            userRequest.Name,
            userRequest.Password,
            store,
            Role.Admin
        );

        store.AddUser(user);

        userRepository.Add(user);
        storeRepository.Add(store);
    }

    public void Delete()
    {
        var store = LoggedUser.Get().Store;

        foreach (var user in store.Users)
            userRepository.Delete(user);

        storeRepository.Delete(store);
    }

    public StoreResponse GetStore()
    {
        return ToDTO(LoggedUser.Get().Store);
    }

    public void ChangeName(StoreUpdateRequest request)
    {
        var store = LoggedUser.Get();

        store.ChangeName(request.Name);
    }

    private StoreResponse ToDTO(Store store)
    {
        return new StoreResponse(
            store.Id,
            store.Name
        );
    }
}
