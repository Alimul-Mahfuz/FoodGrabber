namespace FoodGrabber.Cart.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public string ItemType { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public Guid ItemId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
