namespace FoodGrabber.Order.Entities;

public class OrderStatusHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public OrderStatus? PreviousStatus { get; set; }
    public OrderStatus CurrentStatus { get; set; }
    public string ChangedBy { get; set; } = "system";
    public string? Note { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public Order? Order { get; set; }
}
