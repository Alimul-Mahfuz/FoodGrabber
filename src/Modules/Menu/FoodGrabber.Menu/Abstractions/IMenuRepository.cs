using FoodGrabber.Shared.Abstractions;
using MenuEntity = FoodGrabber.Menu.Entities.Menu;

namespace FoodGrabber.Menu.Abstractions;

public interface IMenuRepository : IRepository<MenuEntity, Guid>
{
    Task EnsureStorageAsync(CancellationToken cancellationToken = default);
    Task<MenuEntity?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteAsync(MenuEntity menu, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
