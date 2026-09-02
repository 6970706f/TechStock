namespace TechStock.Application.DTOs;

public record StoreCreateRequest(
    string Name
);

public record StoreChangeNameRequest(
    string Name
);

public record StoreResponse(
    int Id,
    string Name
);
