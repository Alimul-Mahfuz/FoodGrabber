using FoodGrabber.Menu.Contracts;
using FoodGrabber.Shared.Pagination;

namespace FoodGrabber.Menu.Abstractions;

public interface IMenuService
{
    Task<PagedResult<MenuResponse>> GetAllAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);
    Task<MenuResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MenuResponse> CreateAsync(MenuUpsertRequest request, CancellationToken cancellationToken = default);
    Task<MenuResponse?> UpdateAsync(Guid id, MenuUpsertRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
