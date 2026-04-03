namespace FoodGrabber.Order.Entities;

public class DiningTable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BranchId { get; set; }
    public string TableCode { get; set; } = string.Empty;
    public string? FloorName { get; set; }
    public string? SectionName { get; set; }
    public int SeatCount { get; set; }
    public bool IsActive { get; set; } = true;
    public Branch? Branch { get; set; }
}
