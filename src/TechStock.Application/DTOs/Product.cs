namespace TechStock.Application.DTOs;

public record ProductCreateRequest(
    string Name,
    decimal Price,
    int Quantity
);

public record ProductUpdateRequest(
    string Name,
    decimal Price,
    int Quantity
);

public record ProductResponse(
    int Id,
    string Name,
    decimal Price,
    int Quantity
);
