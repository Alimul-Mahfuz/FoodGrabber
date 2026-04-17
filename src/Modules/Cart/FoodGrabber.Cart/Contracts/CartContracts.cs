namespace FoodGrabber.Cart.Contracts;

public sealed record CartContracts();

public sealed record CartItemRequest(string Type, int Quantity, string ItemId);

public sealed record AddToCartRequest(
        List<CartItemRequest> CartItems
    );


public sealed record CartResponse(
     Guid Id,
     Guid? UserId,
     List<CartItemRequest> CartItems,
     int TotalItems
    );
