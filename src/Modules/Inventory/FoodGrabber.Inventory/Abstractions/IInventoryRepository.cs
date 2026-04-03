using FoodGrabber.Product.Entities;
using ProductEntity = FoodGrabber.Product.Entities.Product;

namespace FoodGrabber.Inventory.Abstractions;

public interface IInventoryRepository
{
    Task<ProductEntity?> GetProductAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductStockEntry>> GetEntriesAsync(Guid productId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> CountEntriesAsync(Guid productId, CancellationToken cancellationToken = default);
    Task AddStockEntryAsync(ProductStockEntry entry, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
