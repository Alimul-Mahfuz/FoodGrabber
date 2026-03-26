using Microsoft.AspNetCore.Identity;

namespace FoodGrabber.Identity.Entites
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
