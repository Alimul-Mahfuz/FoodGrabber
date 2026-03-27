using FoodGrabber.Order.Contracts;

namespace FoodGrabber.Order.Abstractions
{
    public interface IOrderService
    {
        Task<IReadOnlyList<OrderResponse>> GetAllAsync(CancellationToken ct = default);
        Task<OrderResponse> CreateAsync(OrderUpsertRequest request, CancellationToken ct = default);
    }
}
