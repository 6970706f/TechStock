using ErrorOr;
using TechStock.Application.DTOs;
using TechStock.Domain.Entities;
using TechStock.Domain.Enums;
using TechStock.Infrastructure.Repositories;
using TechStock.Infrastructure.Services;

namespace TechStock.Application.Services;

public class StoreService(
    StoreRepository storeRepository,
    UserRepository userRepository,
    PasswordService passwordService
)
{
    public ErrorOr<Created> Add(StoreCreateRequest storeRequest, UserCreateRequest userRequest)
    {
        var store = new Store(storeRequest.Name);

        if (string.IsNullOrWhiteSpace(userRequest.Name))
            return Error.Validation(
                code: "User.NameValidation",
                description: "invalid credentials"
            );
        
        if (string.IsNullOrWhiteSpace(userRequest.Password) ||
        userRequest.ConfirmPassword != userRequest.Password)
            return Error.Validation(
                code: "User.Password",
                description: "invalid credentials"
            );

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
    }

    public ErrorOr<Deleted> Delete()
    {
        UserIsAuthorized();
        var store = GetStoreErrorOr();

        if (store.IsError)
            return store.Errors;

        foreach (var user in store.Value.Users)
            userRepository.Delete(user);

        storeRepository.Delete(store.Value);

        return Result.Deleted;
    }

    public ErrorOr<StoreResponse> GetStore()
    {
        var store = GetStoreErrorOr();

        if (store.IsError)
            return store.Errors;

        return ToDTO(store.Value);
    }

    public ErrorOr<Updated> ChangeName(StoreUpdateRequest request)
    {
        UserIsAuthorized();
        var store = GetStoreErrorOr();

        if (store.IsError)
            return store.Errors;
        
        if (string.IsNullOrWhiteSpace(request.Name))
            return Error.Validation(
                code: "Store.NameValidation",
                description: "invalid name"
            );

        store.Value.ChangeName(request.Name);

        return Result.Updated;
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
