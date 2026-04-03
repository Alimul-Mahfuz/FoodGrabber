using FoodGrabber.Product.Contracts;
using FoodGrabber.Shared.Pagination;

namespace FoodGrabber.Product.Abstractions;

public interface IProductService
{
    Task<PagedResult<ProductResponse>> GetAllAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);
    Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductResponse> CreateAsync(ProductCreateRequest request, string userId, CancellationToken cancellationToken = default);
    Task<ProductResponse?> UpdateAsync(Guid id, ProductUpsertRequest request, string userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
