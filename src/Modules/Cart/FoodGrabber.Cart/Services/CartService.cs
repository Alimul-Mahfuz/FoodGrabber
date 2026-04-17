using FoodGrabber.Cart.Abstractions;
using FoodGrabber.Cart.Contracts;
using FoodGrabber.Shared.Result;
using CartEntity = FoodGrabber.Cart.Entities.Cart;
using CartItemEntity = FoodGrabber.Cart.Entities.CartItem;

namespace FoodGrabber.Cart.Services;

public sealed class CartService(ICartRepository cartRepository) : ICartService
{
    public async Task<Result<CartResponse>> CreateCartAsync(AddToCartRequest addToCartRequest, Guid userId, CancellationToken ctx = default)
    {
        var cart = new CartEntity
        {
            UserId = userId,
            CartItems = addToCartRequest.CartItems.Select(x => new CartItemEntity
            {
                ItemType = x.Type,
                Quantity = x.Quantity,
                ItemId = Guid.TryParse(x.ItemId, out var itemId) ? itemId : Guid.Empty
            }).ToList()
        };

        var result = await cartRepository.AddTheItemToCartAsync(cart, ctx);

        var value = new CartResponse(
            result.Id,
            result.UserId,
            result.CartItems.Select(ct => new CartItemRequest(
                ct.ItemType,
                ct.Quantity,
                ct.ItemId.ToString())).ToList(),
            result.CartItems.Count);

        return Result<CartResponse>.Success(value);
    }
}
