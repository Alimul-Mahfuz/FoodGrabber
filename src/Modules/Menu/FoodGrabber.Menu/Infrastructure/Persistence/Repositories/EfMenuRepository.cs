using FoodGrabber.Menu.Abstractions;
using Microsoft.EntityFrameworkCore;
using MenuEntity = FoodGrabber.Menu.Entities.Menu;

namespace FoodGrabber.Menu.Infrastructure.Persistence.Repositories;

public sealed class EfMenuRepository(DbContext dbContext) : IMenuRepository
{
    public Task EnsureStorageAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public async Task<IReadOnlyList<MenuEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<MenuEntity>()
            .AsNoTracking()
            .Include(menu => menu.Products)
            .ToListAsync(cancellationToken);
    }

    public async Task<MenuEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<MenuEntity>()
            .AsNoTracking()
            .Include(menu => menu.Products)
            .FirstOrDefaultAsync(menu => menu.Id == id, cancellationToken);
    }

    public async Task<MenuEntity?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<MenuEntity>()
            .Include(menu => menu.Products)
            .FirstOrDefaultAsync(menu => menu.Id == id, cancellationToken);
    }

    public async Task AddAsync(MenuEntity menu, CancellationToken cancellationToken = default)
    {
        dbContext.Set<MenuEntity>().Add(menu);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(MenuEntity menu, CancellationToken cancellationToken = default)
    {
        dbContext.Set<MenuEntity>().Remove(menu);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
