namespace FoodGrabber.Menu.Entities;

public class MenuCategoryProduct
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MenuCategoryId { get; set; }
    public Guid ProductId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public MenuCategory? MenuCategory { get; set; }
}
