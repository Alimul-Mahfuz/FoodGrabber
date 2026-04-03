namespace FoodGrabber.Order.Entities;

public class OrderDetails
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public Guid PaymentId { get; set; } = Guid.Empty;
    public Guid? MenuId { get; set; }
    public Guid? ProductId { get; set; }
    public string ItemNameSnapshot { get; set; } = string.Empty;
    public string PriceSource { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public Order? Order { get; set; }
    public ICollection<OrderItemModifier> Modifiers { get; set; } = new List<OrderItemModifier>();
}
