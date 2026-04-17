using FoodGrabber.Cart.Contracts;
using FoodGrabber.Shared.Result;

namespace FoodGrabber.Cart.Abstractions;

public interface ICartService
{
    Task<Result<CartResponse>> CreateCartAsync(AddToCartRequest addToCartRequest, Guid userId, CancellationToken ctx = default);

}
