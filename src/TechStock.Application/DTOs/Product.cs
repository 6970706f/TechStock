using TechStock.Domain.Enums;

namespace TechStock.Application.DTOs;

public record ProductCreateRequest(
    string Name,
    decimal Price,
    int Quantity
);

public record ProductUpdateRequest(
    string Name,
    decimal Price
);

public record ProductMovementRequest(
    int Quantity,
    ProductMovementType Type
);

public record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    int Quantity
);
