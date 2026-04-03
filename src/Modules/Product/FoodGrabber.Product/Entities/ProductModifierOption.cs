namespace FoodGrabber.Product.Entities;

public class ProductModifierOption
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductModifierGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PriceDelta { get; set; }
    public bool IsDefault { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public ProductModifierGroup? ProductModifierGroup { get; set; }
}
