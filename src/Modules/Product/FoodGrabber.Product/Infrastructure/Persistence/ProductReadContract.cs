using FoodGrabber.Product.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FoodGrabber.Product.Infrastructure.Persistence;

public sealed class ProductReadContract(DbContext dbContext) : IProductReadContract
{
    public async Task<ProductPricingResponse?> GetPricingAsync(Guid productId, CancellationToken ct = default)
    {
        return await dbContext.Set<Entities.Product>()
            .AsNoTracking()
            .Where(product => product.Id == productId)
            .Select(product => new ProductPricingResponse(
                product.Id,
                product.Name,
                product.SellingPrice,
                product.IsActive))
            .FirstOrDefaultAsync(ct);
    }
}
