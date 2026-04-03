using FoodGrabber.Inventory.Abstractions;
using FoodGrabber.Inventory.Contracts;
using FoodGrabber.Inventory.Exceptions;
using FoodGrabber.Product.Entities;
using FoodGrabber.Shared.Pagination;

namespace FoodGrabber.Inventory.Services;

public sealed class InventoryService(IInventoryRepository inventoryRepository) : IInventoryService
{
    private static readonly HashSet<string> AllowedMovementTypes = ["in", "out", "adjustment"];

    public async Task<PagedResult<StockEntryResponse>> GetEntriesAsync(
        Guid productId,
        PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default)
    {
        var page = paginationQuery.NormalizedPage;
        var pageSize = paginationQuery.NormalizedPageSize;

        var entries = await inventoryRepository.GetEntriesAsync(productId, page, pageSize, cancellationToken);
        var totalCount = await inventoryRepository.CountEntriesAsync(productId, cancellationToken);

        return new PagedResult<StockEntryResponse>
        {
            Items = entries.Select(MapEntry).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<InventoryAdjustmentResponse> AdjustStockAsync(
        Guid productId,
        StockAdjustmentRequest request,
        string userId,
        CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var product = await inventoryRepository.GetProductAsync(productId, cancellationToken);
        if (product is null)
        {
            throw new InventoryException("Product not found.");
        }

        var movementType = request.MovementType.Trim().ToLowerInvariant();
        var unit = string.IsNullOrWhiteSpace(request.Unit)
            ? product.StockUnit
            : request.Unit.Trim().ToLowerInvariant();

        var quantityDelta = movementType == "out" ? -request.Quantity : request.Quantity;
        var nextStock = product.CurrentStock + quantityDelta;

        if (nextStock < 0)
        {
            throw new InventoryException("Insufficient stock.");
        }

        product.CurrentStock = nextStock;
        product.StockUnit = unit;
        product.UpdatedAt = DateTime.UtcNow;

        var entry = new ProductStockEntry
        {
            ProductId = productId,
            Quantity = request.Quantity,
            MovementType = movementType,
            Unit = unit,
            Notes = request.Notes?.Trim(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await inventoryRepository.AddStockEntryAsync(entry, cancellationToken);
        await inventoryRepository.SaveChangesAsync(cancellationToken);

        return new InventoryAdjustmentResponse(
            product.Id,
            product.Name,
            product.CurrentStock,
            product.StockUnit,
            MapEntry(entry));
    }

    private static void ValidateRequest(StockAdjustmentRequest request)
    {
        if (request.Quantity <= 0)
        {
            throw new InventoryException("Quantity must be greater than zero.");
        }

        var movementType = request.MovementType?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(movementType) || !AllowedMovementTypes.Contains(movementType))
        {
            throw new InventoryException("MovementType must be one of: in, out, adjustment.");
        }
    }

    private static StockEntryResponse MapEntry(ProductStockEntry entry)
    {
        return new StockEntryResponse(
            entry.Id,
            entry.ProductId,
            entry.Quantity,
            entry.MovementType,
            entry.Unit,
            entry.Notes,
            entry.UserId,
            entry.CreatedAt);
    }
}
