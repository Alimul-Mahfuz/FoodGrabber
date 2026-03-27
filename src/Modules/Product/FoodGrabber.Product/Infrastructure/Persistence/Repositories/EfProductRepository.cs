using FoodGrabber.Product.Abstractions;
using Microsoft.EntityFrameworkCore;
using ProductEntity = FoodGrabber.Product.Entities.Product;

namespace FoodGrabber.Product.Infrastructure.Persistence.Repositories;

public sealed class EfProductRepository(DbContext dbContext) : IProductRepository
{
    public Task EnsureStorageAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Set<ProductEntity>().AnyAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProductEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<ProductEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<ProductEntity>().FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    public async Task AddAsync(ProductEntity product, CancellationToken cancellationToken = default)
    {
        dbContext.Set<ProductEntity>().Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ProductEntity product, CancellationToken cancellationToken = default)
    {
        dbContext.Set<ProductEntity>().Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ProductEntity product, CancellationToken cancellationToken = default)
    {
        dbContext.Set<ProductEntity>().Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
