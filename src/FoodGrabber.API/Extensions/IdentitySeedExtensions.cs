using FoodGrabber.Infrastructure.Data;
using FoodGrabber.Identity.Entites;
using FoodGrabber.Shared.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodGrabber.API.Extensions;

public static class IdentitySeedExtensions
{
    public static async Task SeedIdentityAsync(this IServiceProvider services, IConfiguration configuration)
    {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;

        var dbContext = scopedServices.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();

        var roleManager = scopedServices.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();

        await EnsureRoleExistsAsync(roleManager, RoleNames.Admin);
        await EnsureRoleExistsAsync(roleManager, RoleNames.Customer);

        var adminEmail = configuration["DefaultAdmin:Email"] ?? "admin@foodgrabber.local";
        var adminPassword = configuration["DefaultAdmin:Password"] ?? "Admin@123456";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminUser, adminPassword);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create default admin user. {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(adminUser, RoleNames.Admin))
        {
            await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);
        }
    }

    private static async Task EnsureRoleExistsAsync(RoleManager<ApplicationRole> roleManager, string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
        {
            return;
        }

        var result = await roleManager.CreateAsync(new ApplicationRole
        {
            Name = roleName,
            Description = $"{roleName} role"
        });

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create role '{roleName}'. {errors}");
        }
    }
}
