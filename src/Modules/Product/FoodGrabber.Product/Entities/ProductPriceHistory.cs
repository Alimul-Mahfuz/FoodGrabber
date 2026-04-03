namespace FoodGrabber.Product.Entities;

public class ProductPriceHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public decimal BasePrice { get; set; }
    public decimal SellingPrice { get; set; }
    public string? Reason { get; set; }
    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;
    public DateTime? EffectiveTo { get; set; }
    public Product? Product { get; set; }
}
