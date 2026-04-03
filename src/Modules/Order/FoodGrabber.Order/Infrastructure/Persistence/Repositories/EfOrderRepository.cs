using FoodGrabber.Order.Abstractions;
using Microsoft.EntityFrameworkCore;
using OrderEntity = FoodGrabber.Order.Entities.Order;

namespace FoodGrabber.Order.Infrastructure.Persistence.Repositories;

public sealed class EfOrderRepository(DbContext dbContext) : IOrderRepository
{
    public Task EnsureStorageAsync(CancellationToken ct = default) => Task.CompletedTask;

    public Task<int> CountAsync(CancellationToken ct = default)
    {
        return dbContext.Set<OrderEntity>().CountAsync(ct);
    }

    public Task<OrderEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return dbContext.Set<OrderEntity>()
            .AsNoTracking()
            .Include(order => order.OrderDetails)
            .FirstOrDefaultAsync(order => order.Id == id, ct);
    }

    public async Task AddAsync(OrderEntity order, CancellationToken ct = default)
    {
        dbContext.Set<OrderEntity>().Add(order);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<OrderEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await dbContext.Set<OrderEntity>()
            .AsNoTracking()
            .Include(order => order.OrderDetails)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<OrderEntity>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await dbContext.Set<OrderEntity>()
            .AsNoTracking()
            .Include(order => order.OrderDetails)
            .OrderByDescending(order => order.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task UpdateAsync(OrderEntity entity, CancellationToken ct = default)
    {
        dbContext.Set<OrderEntity>().Update(entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var order = await dbContext.Set<OrderEntity>()
            .FirstOrDefaultAsync(currentOrder => currentOrder.Id == id, ct);

        if (order is null)
        {
            return;
        }

        dbContext.Set<OrderEntity>().Remove(order);
        await dbContext.SaveChangesAsync(ct);
    }
}
