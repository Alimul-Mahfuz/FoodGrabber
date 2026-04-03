namespace FoodGrabber.Order.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? CustomerId { get; set; }
    public Guid UserId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? DiningTableId { get; set; }
    public OrderType OrderType { get; set; } = OrderType.Online;
    public ServiceType ServiceType { get; set; } = ServiceType.Delivery;
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public string? CancellationReason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public decimal SubtotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalPrice { get; set; }
    public Branch? Branch { get; set; }
    public DiningTable? DiningTable { get; set; }
    public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    public ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
}

public enum OrderStatus
{
    Created,
    Confirmed,
    Preparing,
    Ready,
    Served,
    Dispatched,
    Completed,
    Cancelled,
    Unpaid,
    Paid,
    Delivered
}

public enum ServiceType
{
    DineIn,
    Takeaway,
    Delivery
}

public enum OrderType
{
    Online,
    Offline
}
