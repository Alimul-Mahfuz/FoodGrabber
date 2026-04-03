using FoodGrabber.Inventory.Contracts;
using FoodGrabber.Shared.Pagination;

namespace FoodGrabber.Inventory.Abstractions;

public interface IInventoryService
{
    Task<PagedResult<StockEntryResponse>> GetEntriesAsync(
        Guid productId,
        PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default);

    Task<InventoryAdjustmentResponse> AdjustStockAsync(
        Guid productId,
        StockAdjustmentRequest request,
        string userId,
        CancellationToken cancellationToken = default);
}
