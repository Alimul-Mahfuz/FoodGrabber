using FoodGrabber.Menu.Contracts;

namespace FoodGrabber.Menu.Abstractions;

public interface IMenuService
{
    Task<IReadOnlyList<MenuResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MenuResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MenuResponse> CreateAsync(MenuUpsertRequest request, CancellationToken cancellationToken = default);
    Task<MenuResponse?> UpdateAsync(Guid id, MenuUpsertRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
