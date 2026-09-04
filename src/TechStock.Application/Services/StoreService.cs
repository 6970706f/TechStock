using ErrorOr;
using TechStock.Application.DTOs;
using TechStock.Application.Validators;
using TechStock.Domain.Entities;
using TechStock.Domain.Enums;
using TechStock.Infrastructure.Repositories;
using TechStock.Infrastructure.Services;

namespace TechStock.Application.Services;

public class StoreService(
    StoreRepository storeRepository,
    UserRepository userRepository,
    PasswordService passwordService,
    StoreValidators storeValidators
)
{
    public ErrorOr<Created> Add(StoreCreateRequest storeRequest, UserCreateRequest userRequest)
    {
        return storeValidators.AddValidator(storeRequest, userRequest)
            .Then(_ =>
            {
                var store = new Store(storeRequest.Name);

                var user = new User(
                    userRequest.Name,
                    passwordService.HashPassword(userRequest.Password),
                    store,
                    Role.Admin
                );

                store.AddUser(user);

                userRepository.Add(user);
                storeRepository.Add(store);

                return Result.Created;
            });
    }

    public ErrorOr<Deleted> Delete()
    {
        return UserIsAuthorized()
            .Then(_ => GetStoreErrorOr()
            .Then(store =>
            {
                foreach (var user in store.Users)
                    userRepository.Delete(user);

                storeRepository.Delete(store);

                return Result.Deleted;
            }));
    }

    public ErrorOr<StoreResponse> GetStore()
    {
        return GetStoreErrorOr()
            .Then(store =>
            {
                return ToDTO(store);
            });
    }

    public ErrorOr<Updated> ChangeName(StoreUpdateRequest request)
    {
        return UserIsAuthorized()
            .Then(_ => GetStoreErrorOr()
            .Then(store =>
            {
                store.ChangeName(request.Name);

                return Result.Updated;
            }));
    }

    private ErrorOr<Store> GetStoreErrorOr()
    {
        var user = LoggedUser.Get();

        if (user is null)
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "user unauthorized"
            );
        
        return user.Store;
    }

    private ErrorOr<Success> UserIsAuthorized()
    {
        var user = LoggedUser.Get();

        if (user is null || user.Role != Role.Admin)
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "user unauthorized"
            );
        
        return Result.Success;
    }

    private StoreResponse ToDTO(Store store)
    {
        return new StoreResponse(
            store.Id,
            store.Name
        );
    }
}
