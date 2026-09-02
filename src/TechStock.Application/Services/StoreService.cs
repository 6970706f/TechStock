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

        userRepository.Add(user);
        storeRepository.Add(store);
    }

    public void Delete()
    {
        storeRepository.Delete(GetLoggedStoreOrThrow());
    }

    public StoreResponse GetStore()
    {
        return ToDTO(GetLoggedStoreOrThrow());
    }

    public void ChangeName(StoreChangeNameRequest request)
    {
        var store = GetLoggedStoreOrThrow();

        store.ChangeName(request.Name);
    }

    private Store GetLoggedStoreOrThrow()
    {
        User user = LoggedUser.Current
            ?? throw new Exception();
        
        return user.Store;
    }

    private StoreResponse ToDTO(Store store)
    {
        return new StoreResponse(
            store.Id,
            store.Name
        );
    }
}
