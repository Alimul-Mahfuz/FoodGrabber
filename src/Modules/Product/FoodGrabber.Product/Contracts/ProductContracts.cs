namespace FoodGrabber.Product.Contracts;

public interface IProductReadContract
{
    Task<ProductPricingResponse?> GetPricingAsync(Guid productId, CancellationToken ct = default);
}

public sealed record ProductPricingResponse(
    Guid Id,
    string Name,
    decimal UnitPrice,
    bool IsActive);

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
