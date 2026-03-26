using Microsoft.AspNetCore.Identity;

namespace FoodGrabber.Identity.Entites
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
    }
}
