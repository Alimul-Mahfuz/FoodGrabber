namespace FoodGrabber.Product.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal CurrentStock { get; set; }
    public string StockUnit { get; set; } = "piece";
    public decimal BasePrice { get; set; }
    public decimal SellingPrice { get; set; }
    public string? Image { get; set; }
    public string Tags { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ProductStockEntry> StockEntries { get; set; } = new List<ProductStockEntry>();
}
