using FoodGrabber.Order.Abstractions;
using FoodGrabber.Order.Contracts;
using FoodGrabber.Order.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FoodGrabber.Order.Infrastructure.Persistence;

public sealed class OrderPricingGateway(DbContext dbContext) : IOrderPricingGateway
{
    public async Task<PricedOrderItem> GetProductPriceAsync(Guid productId, int quantity, CancellationToken ct = default)
    {
        var product = await dbContext.Set<FoodGrabber.Product.Entities.Product>()
            .AsNoTracking()
            .FirstOrDefaultAsync(currentProduct => currentProduct.Id == productId && currentProduct.IsActive, ct);

        if (product is null)
        {
            throw new OrderModuleException($"Product '{productId}' was not found.");
        }

        var unitPrice = product.SellingPrice;
        return new PricedOrderItem(product.Id, quantity, unitPrice, unitPrice * quantity, "Product");
    }

    public async Task<PricedOrderItem> GetMenuPriceAsync(Guid menuId, int quantity, CancellationToken ct = default)
    {
        var menu = await dbContext.Set<FoodGrabber.Menu.Entities.Menu>()
            .AsNoTracking()
            .FirstOrDefaultAsync(currentMenu => currentMenu.Id == menuId && currentMenu.IsActive, ct);

        if (menu is null)
        {
            throw new OrderModuleException($"Menu '{menuId}' was not found.");
        }

        var unitPrice = menu.SellingPrice;
        return new PricedOrderItem(menu.Id, quantity, unitPrice, unitPrice * quantity, "Menu");
    }
}
