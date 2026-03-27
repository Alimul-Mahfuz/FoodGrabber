using MenuEntity = FoodGrabber.Menu.Entities.Menu;

namespace FoodGrabber.Menu.Abstractions;

public interface IMenuRepository
{
    Task EnsureStorageAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MenuEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MenuEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MenuEntity?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(MenuEntity menu, CancellationToken cancellationToken = default);
    Task DeleteAsync(MenuEntity menu, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
