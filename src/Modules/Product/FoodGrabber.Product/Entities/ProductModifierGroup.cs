namespace FoodGrabber.Product.Entities;

public class ProductModifierGroup
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MinSelections { get; set; }
    public int MaxSelections { get; set; } = 1;
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public Product? Product { get; set; }
    public ICollection<ProductModifierOption> Options { get; set; } = new List<ProductModifierOption>();
}
