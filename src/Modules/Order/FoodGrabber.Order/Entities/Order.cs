namespace FoodGrabber.Order.Entities
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? CustomerId { get; set; }
        public Guid UserId { get; set; }
        public OrderType OrderType { get; set; } = OrderType.Online;
        public OrderStatus Status { get; set; } = OrderStatus.Unpaid;
        public string? CancellationReason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();

    }


    public enum OrderStatus
    {
        Paid,
        Unpaid,
        Confirmed,
        Cancelled,
        Delivered
    }

    public enum OrderType
    {
        Online,
        Offline
    }


}
