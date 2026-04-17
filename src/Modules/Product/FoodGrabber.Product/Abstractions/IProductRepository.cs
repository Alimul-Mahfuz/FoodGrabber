using FoodGrabber.Shared.Abstractions;
using ProductEntity = FoodGrabber.Product.Entities.Product;

namespace FoodGrabber.Product.Abstractions;

public interface IProductRepository : IRepository<FoodGrabber.Product.Entities.Product, Guid>
{
    Task EnsureStorageAsync(CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(FoodGrabber.Product.Entities.Product product, CancellationToken cancellationToken = default);
    Task AddWithStockEntryAsync(
        FoodGrabber.Product.Entities.Product product,
        FoodGrabber.Product.Entities.ProductStockEntry stockEntry,
        CancellationToken cancellationToken = default);


    Task<List<ProductEntity>> GetAllProductPricesByIds(Guid[] ProductId, CancellationToken ctx = default);
}
