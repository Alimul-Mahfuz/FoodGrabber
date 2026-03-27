namespace FoodGrabber.Product.Abstractions;

public interface IProductRepository
{
    Task EnsureStorageAsync(CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FoodGrabber.Product.Entities.Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FoodGrabber.Product.Entities.Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(FoodGrabber.Product.Entities.Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(FoodGrabber.Product.Entities.Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(FoodGrabber.Product.Entities.Product product, CancellationToken cancellationToken = default);
}
