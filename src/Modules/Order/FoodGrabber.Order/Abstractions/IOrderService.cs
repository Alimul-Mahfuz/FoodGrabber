using FoodGrabber.Order.Contracts;
using FoodGrabber.Shared.Pagination;

namespace FoodGrabber.Order.Abstractions;

public interface IOrderService
{
    Task<PagedResult<OrderResponse>> GetAllAsync(PaginationQuery paginationQuery, CancellationToken ct = default);
    Task<OrderResponse> CreateAsync(OrderUpsertRequest request, CancellationToken ct = default);
}
