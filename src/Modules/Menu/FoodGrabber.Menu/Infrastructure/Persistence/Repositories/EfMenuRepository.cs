using FoodGrabber.Menu.Abstractions;
using Microsoft.EntityFrameworkCore;
using MenuEntity = FoodGrabber.Menu.Entities.Menu;

namespace FoodGrabber.Menu.Infrastructure.Persistence.Repositories;

public sealed class EfMenuRepository(DbContext dbContext) : IMenuRepository
{
    public Task EnsureStorageAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task<int> CountAsync(CancellationToken ct = default)
    {
        return dbContext.Set<MenuEntity>().CountAsync(ct);
    }

    public async Task<IReadOnlyList<MenuEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<MenuEntity>()
            .AsNoTracking()
            .Include(menu => menu.Products)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MenuEntity>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await dbContext.Set<MenuEntity>()
            .AsNoTracking()
            .Include(menu => menu.Products)
            .OrderByDescending(menu => menu.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
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

    public async Task UpdateAsync(MenuEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<MenuEntity>().Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var menu = await GetByIdForUpdateAsync(id, cancellationToken);
        if (menu is null)
        {
            return;
        }

        await DeleteAsync(menu, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
