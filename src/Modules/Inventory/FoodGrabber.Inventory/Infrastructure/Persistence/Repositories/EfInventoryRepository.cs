using FoodGrabber.Inventory.Abstractions;
using FoodGrabber.Product.Entities;
using Microsoft.EntityFrameworkCore;
using ProductEntity = FoodGrabber.Product.Entities.Product;

namespace FoodGrabber.Inventory.Infrastructure.Persistence.Repositories;

public sealed class EfInventoryRepository(DbContext dbContext) : IInventoryRepository
{
    public Task<ProductEntity?> GetProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<ProductEntity>().FirstOrDefaultAsync(product => product.Id == productId, cancellationToken);
    }

    public async Task<IReadOnlyList<ProductStockEntry>> GetEntriesAsync(
        Guid productId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<ProductStockEntry>()
            .AsNoTracking()
            .Where(entry => entry.ProductId == productId)
            .OrderByDescending(entry => entry.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountEntriesAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<ProductStockEntry>()
            .CountAsync(entry => entry.ProductId == productId, cancellationToken);
    }

    public Task AddStockEntryAsync(ProductStockEntry entry, CancellationToken cancellationToken = default)
    {
        dbContext.Set<ProductStockEntry>().Add(entry);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
