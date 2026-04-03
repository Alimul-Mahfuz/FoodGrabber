namespace FoodGrabber.Order.Entities;

public class OrderItemModifier
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderDetailsId { get; set; }
    public string ModifierGroupName { get; set; } = string.Empty;
    public string ModifierOptionName { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal UnitPriceDelta { get; set; }
    public decimal TotalPriceDelta { get; set; }
    public OrderDetails? OrderDetails { get; set; }
}
