using FoodGrabber.Product.Contracts;

namespace FoodGrabber.Product.Abstractions;

public interface IProductService
{
    Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductResponse> CreateAsync(ProductUpsertRequest request, string userId, CancellationToken cancellationToken = default);
    Task<ProductResponse?> UpdateAsync(Guid id, ProductUpsertRequest request, string userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
