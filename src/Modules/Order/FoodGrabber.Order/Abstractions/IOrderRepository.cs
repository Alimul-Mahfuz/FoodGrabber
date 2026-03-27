using FoodGrabber.Shared.Abstractions;
using OrderEntity = FoodGrabber.Order.Entities.Order;

namespace FoodGrabber.Order.Abstractions;

public interface IOrderRepository : IRepository<OrderEntity, Guid>
{
    Task EnsureStorageAsync(CancellationToken ct = default);
}
