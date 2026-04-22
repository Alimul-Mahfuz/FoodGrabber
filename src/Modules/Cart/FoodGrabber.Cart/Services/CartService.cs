using FoodGrabber.Cart.Abstractions;
using FoodGrabber.Cart.Contracts;
using FoodGrabber.Cart.Exceptions;
using FoodGrabber.Product.Contracts;
using FoodGrabber.Shared.Result;
using CartEntity = FoodGrabber.Cart.Entities.Cart;
using CartItemEntity = FoodGrabber.Cart.Entities.CartItem;

namespace FoodGrabber.Cart.Services;

public sealed class CartService(
    ICartRepository cartRepository,
    IProductReadContract productReadContract) : ICartService
{
    public async Task<Result<CartResponse>> CreateCartAsync(AddToCartRequest addToCartRequest, Guid userId, CancellationToken ctx = default)
    {
        try
        {
            var productIds = addToCartRequest.CartItems
                .Where(x => Guid.TryParse(x.ItemId, out _))
                .Select(x => Guid.Parse(x.ItemId))
                .ToArray();

            var productPrices = await productReadContract.GetPricingByIdsAsync(productIds, ctx);
            var priceMap = productPrices.ToDictionary(p => p.Id, p => p.UnitPrice);

            var cart = new CartEntity
            {
                UserId = userId,
                CartItems = addToCartRequest.CartItems.Select(x =>
                {
                    var itemId = Guid.TryParse(x.ItemId, out var id) ? id : Guid.Empty;
                    var unitPrice = priceMap.ContainsKey(itemId) ? priceMap[itemId] : 0m;

                    return new CartItemEntity
                    {
                        ItemType = x.Type,
                        Quantity = x.Quantity,
                        ItemId = itemId,
                        UnitPrice = unitPrice
                    };
                }).ToList()
            };

            var result = await cartRepository.AddTheItemToCartAsync(cart, ctx);

            var value = new CartResponse(
                result.Id,
                result.UserId,
                result.CartItems.Select(ct => new CartItemRequest(
                    ct.ItemType,
                    ct.Quantity,
                    ct.ItemId.ToString(),
                    ct.UnitPrice)).ToList(),
                result.CartItems.Count);

            return Result<CartResponse>.Success(value);
        }
        catch (Exception ex)
        {
            return Result<CartResponse>.Failure($"Failed to add items to cart: {ex.Message}");
        }
    }

    public async Task<Result<bool>> RemoveItemFromCart(RemoveItemFromCartRequest removeItemFromCartRequest, string UserId, CancellationToken ctx = default)
    {
        if (!Guid.TryParse(removeItemFromCartRequest.ItemId, out var itemId))
        {
            throw new CartException("ItemId invalid");
        }
        if (!Guid.TryParse(UserId, out var userId))
        {
            throw new CartException("User not found");

        }

        if (removeItemFromCartRequest.Quantity < 0)
        {
            throw new CartException("Item's quantity cannot be negative");
        }

        var status = await cartRepository.RemoveItemFromCartAsync(itemId, userId, removeItemFromCartRequest.Quantity, ctx);

        return Result<bool>.Success(status);

    }

    public async Task<Result<CartResponse>> ViewMyCart(Guid UserId, CancellationToken ctx = default)
    {
        var result = await cartRepository.ShowCart(UserId, ctx);
        if (result == null)
        {
            Result<CartResponse?>.Success(null);
        }
        var value = new CartResponse(
                result.Id,
                result.UserId,
                result.CartItems.Select(ct => new CartItemRequest(
                    ct.ItemType,
                    ct.Quantity,
                    ct.ItemId.ToString(),
                    ct.UnitPrice)).ToList(),
                result.CartItems.Count);

        return Result<CartResponse>.Success(value);
    }
}
