namespace FoodGrabber.Product.Entities;

public class ProductAvailabilityWindow
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsAvailable { get; set; } = true;
    public Product? Product { get; set; }
}
