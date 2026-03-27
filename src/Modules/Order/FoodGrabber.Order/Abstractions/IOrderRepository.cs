using OrderEntity = FoodGrabber.Order.Entities.Order;

namespace FoodGrabber.Order.Abstractions
{
    public interface IOrderRepository
    {
        Task<IReadOnlyList<OrderEntity>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(OrderEntity order, CancellationToken ct = default);
        Task EnsureStorageAsync(CancellationToken ct = default);

    }
}
