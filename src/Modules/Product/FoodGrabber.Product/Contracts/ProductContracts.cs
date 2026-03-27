namespace FoodGrabber.Product.Contracts;

public sealed record ProductUpsertRequest(
    string Name,
    string Description,
    int Quantity,
    decimal BasePrice,
    decimal SellingPrice,
    string? Image,
    string[]? Tags,
    bool IsActive);

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    int Quantity,
    decimal BasePrice,
    decimal SellingPrice,
    string? Image,
    IEnumerable<string> Tags,
    bool IsActive,
    string UserId,
    DateTime CreatedAt,
    DateTime UpdatedAt);
