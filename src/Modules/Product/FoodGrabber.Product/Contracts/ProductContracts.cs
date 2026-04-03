using Microsoft.AspNetCore.Http;

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
    string StockUnit,
    decimal BasePrice,
    decimal SellingPrice,
    string? Image,
    string[]? Tags,
    bool IsActive);

public sealed class ProductCreateFormRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal InitialStock { get; init; }
    public string StockUnit { get; init; } = "piece";
    public decimal BasePrice { get; init; }
    public decimal SellingPrice { get; init; }
    public IFormFile Image { get; init; } = default!;
    public string[]? Tags { get; init; }
    public bool IsActive { get; init; } = true;
}

public sealed record ProductCreateRequest(
    string Name,
    string Description,
    decimal InitialStock,
    string StockUnit,
    decimal BasePrice,
    decimal SellingPrice,
    string? Image,
    string[]? Tags,
    bool IsActive);

public sealed class ProductUpdateFormRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string StockUnit { get; init; } = "piece";
    public decimal BasePrice { get; init; }
    public decimal SellingPrice { get; init; }
    public IFormFile? Image { get; init; }
    public string[]? Tags { get; init; }
    public bool IsActive { get; init; } = true;
}

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal CurrentStock,
    string StockUnit,
    decimal BasePrice,
    decimal SellingPrice,
    string? Image,
    IEnumerable<string> Tags,
    bool IsActive,
    string UserId,
    DateTime CreatedAt,
    DateTime UpdatedAt);
