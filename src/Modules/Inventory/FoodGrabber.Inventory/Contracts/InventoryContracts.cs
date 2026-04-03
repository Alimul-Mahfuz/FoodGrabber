using FoodGrabber.Shared.Pagination;

namespace FoodGrabber.Inventory.Contracts;

public sealed record StockAdjustmentRequest(
    decimal Quantity,
    string MovementType,
    string? Unit,
    string? Notes);

public sealed record StockEntryResponse(
    Guid Id,
    Guid ProductId,
    decimal Quantity,
    string MovementType,
    string Unit,
    string? Notes,
    string UserId,
    DateTime CreatedAt);

public sealed record InventoryAdjustmentResponse(
    Guid ProductId,
    string ProductName,
    decimal CurrentStock,
    string StockUnit,
    StockEntryResponse Entry);
