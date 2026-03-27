using FoodGrabber.Menu.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FoodGrabber.Menu.Infrastructure.Persistence;

public sealed class MenuReadContract(DbContext dbContext) : IMenuReadContract
{
    public async Task<MenuPricingResponse?> GetPricingAsync(Guid menuId, CancellationToken ct = default)
    {
        return await dbContext.Set<Entities.Menu>()
            .AsNoTracking()
            .Where(menu => menu.Id == menuId)
            .Select(menu => new MenuPricingResponse(
                menu.Id,
                menu.Name,
                menu.SellingPrice,
                menu.IsActive))
            .FirstOrDefaultAsync(ct);
    }
}
