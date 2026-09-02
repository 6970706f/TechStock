namespace TechStock.Application.DTOs;

public record UserRequest(
    string Name,
    string Password
);

public record UserResponse(
    int Id,
    string Name
);
