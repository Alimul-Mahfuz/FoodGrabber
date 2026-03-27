using FoodGrabber.Order.Abstractions;
using Microsoft.EntityFrameworkCore;
using OrderEntity = FoodGrabber.Order.Entities.Order;

namespace FoodGrabber.Order.Infrastructure.Persistence.Repositories;

public sealed class EfOrderRepository(DbContext dbContext) : IOrderRepository
{
    public Task EnsureStorageAsync(CancellationToken ct = default) => Task.CompletedTask;

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
}
