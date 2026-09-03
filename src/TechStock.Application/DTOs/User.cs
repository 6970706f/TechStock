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

public record LoginRequest(
    string Name,
    string Password
);

public record UserResponse(
    Guid Id,
    string Name
);
