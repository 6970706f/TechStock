namespace TechStock.Application.DTOs;

public record StoreCreateRequest(
    string Name
);

public record StoreUpdateRequest(
    string Name
);

public record StoreResponse(
    int Id,
    string Name
);
