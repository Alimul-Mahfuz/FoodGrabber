using FoodGrabber.Identity.Entites;
using FoodGrabber.Shared.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace FoodGrabber.Identity.Extensions;

public sealed class IdentityAdminUserProvider(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration) : IAdminUserProvider
{
    public async Task<string?> GetDefaultAdminUserIdAsync(CancellationToken cancellationToken = default)
    {
        var adminEmail = configuration["DefaultAdmin:Email"] ?? "admin@foodgrabber.local";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        return adminUser?.Id;
    }
}
