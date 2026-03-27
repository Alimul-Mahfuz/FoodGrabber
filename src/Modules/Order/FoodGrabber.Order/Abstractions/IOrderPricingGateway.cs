using FoodGrabber.Order.Contracts;

namespace FoodGrabber.Order.Abstractions;

public interface IOrderPricingGateway
{
    Task<PricedOrderItem> GetProductPriceAsync(Guid productId, int quantity, CancellationToken ct = default);
    Task<PricedOrderItem> GetMenuPriceAsync(Guid menuId, int quantity, CancellationToken ct = default);
}
