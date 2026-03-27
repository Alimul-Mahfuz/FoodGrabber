namespace FoodGrabber.Menu.Contracts;

public sealed record MenuItemRequest(Guid ProductId, int Quantity);

public sealed record MenuUpsertRequest(
    string Name,
    string Description,
    decimal SellingPrice,
    bool IsActive,
    List<MenuItemRequest>? Products);

public sealed record MenuItemResponse(Guid Id, Guid ProductId, int Quantity);

public sealed record MenuResponse(
    Guid Id,
    string Name,
    string Description,
    decimal SellingPrice,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IEnumerable<MenuItemResponse> Products);
