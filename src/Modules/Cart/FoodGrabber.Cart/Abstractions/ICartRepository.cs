using FoodGrabber.Shared.Abstractions;
using CartEntity = FoodGrabber.Cart.Entities.Cart;

namespace FoodGrabber.Cart.Abstractions;

public interface ICartRepository : IRepository<CartEntity, Guid>
{
    Task<CartEntity> AddTheItemToCartAsync(CartEntity cart, CancellationToken ct = default);
    Task<CartEntity?> IsItemExistOnActiveCart(Guid ItemId, Guid UserId);
    Task<bool> RemoveItemFromCartAsync(Guid ItemId, Guid UserId, int Quantity, CancellationToken ctx = default);
    Task<CartEntity?> ShowCart(Guid UserId, CancellationToken ctx = default);
}
