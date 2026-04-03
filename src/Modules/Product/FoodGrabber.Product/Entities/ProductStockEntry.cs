namespace FoodGrabber.Product.Entities;

public class ProductStockEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public string MovementType { get; set; } = "initial";
    public string Unit { get; set; } = "piece";
    public string? Notes { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Product Product { get; set; } = default!;
}
