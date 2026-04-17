namespace FoodGrabber.Cart.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public CartStatus Status { get; set; } = CartStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

    public enum CartStatus
    {
        Pending,
        Complete
    }
}
