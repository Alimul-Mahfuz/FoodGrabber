using FoodGrabber.Cart.Entities;
using FoodGrabber.Shared.Abstractions;
using CartEntity = FoodGrabber.Cart.Entities.Cart;

namespace FoodGrabber.Cart.Abstractions;

public interface ICartRepository : IRepository<CartEntity, Guid>
{
    Task<CartEntity> AddTheItemToCartAsync(CartEntity cart, CancellationToken ct = default);
}
