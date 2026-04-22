using FoodGrabber.Cart.Contracts;
using FoodGrabber.Shared.Result;

namespace FoodGrabber.Cart.Abstractions;

public interface ICartService
{
    Task<Result<CartResponse>> CreateCartAsync(AddToCartRequest addToCartRequest, Guid userId, CancellationToken ctx = default);
    Task<Result<bool>> RemoveItemFromCart(RemoveItemFromCartRequest removeItemFromCartRequest, string userId, CancellationToken ctx = default);
    Task<Result<CartResponse>> ViewMyCart(Guid userId, CancellationToken ctx = default);

}
