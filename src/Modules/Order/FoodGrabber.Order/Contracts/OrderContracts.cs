namespace FoodGrabber.Order.Contracts
{
    public enum OrderTypeContract
    {
        Online,
        Offline
    }

    public sealed record OrderDetailsRequest(
            Guid? MenuId,
            Guid? ProductId,
            int Quantity

        );
    public sealed record OrderUpsertRequest(
        Guid? CustomerId,
        Guid UserId,
        OrderTypeContract OrderType,
        List<OrderDetailsRequest> OrderDetails
        );

    public sealed record OrderResponse(
        Guid Id,
        Guid? CustomerId,
        Guid UserId,
        string OrderType,
        string OrderStatus,
        string? CancellationReason,
        DateTime CreateAt,
        DateTime UpdatedAt,
        decimal TotalPrice,
        List<OrderDetailsResponse>? OrderDetails = null
        );


    public sealed record OrderDetailsResponse(
        Guid Id,
        Guid? MenuId,
        Guid? ProductId,
        Guid PaymentId,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice
        );

public sealed record PricedOrderItem(
    Guid ItemId,
    string ItemName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    string PriceSource
    );
}
