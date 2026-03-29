namespace FoodGrabber.Identity.Entites;

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string? Email { get; set; }
    public string? Image { get; set; }
    public bool IsActive { get; set; } = true;

    public ApplicationUser? User { get; set; }
}
