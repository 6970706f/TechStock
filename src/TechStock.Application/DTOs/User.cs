namespace TechStock.Application.DTOs;

public record UserCreateRequest(
    string Name,
    string Password,
    string ConfirmPassword
);

public record UserChangeNameRequest(
    string Name
);

public record UserChangePasswordRequest(
    string OldPassword,
    string NewPassword,
    string ConfirmPassword
);

public record UserResponse(
    int Id,
    string Name
);
