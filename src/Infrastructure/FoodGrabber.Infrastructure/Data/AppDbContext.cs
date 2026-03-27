using FoodGrabber.Identity.Entites;
using FoodGrabber.Menu.Infrastructure.Persistence.Configurations;
using FoodGrabber.Order.Infrastructure.Persistence.Configurations;
using FoodGrabber.Product.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodGrabber.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    public DbSet<FoodGrabber.Menu.Entities.Menu> Menus => Set<FoodGrabber.Menu.Entities.Menu>();
    public DbSet<FoodGrabber.Menu.Entities.MenuProduct> MenuProducts => Set<FoodGrabber.Menu.Entities.MenuProduct>();
    public DbSet<FoodGrabber.Product.Entities.Product> Products => Set<FoodGrabber.Product.Entities.Product>();
    public DbSet<FoodGrabber.Order.Entities.Order> Orders => Set<FoodGrabber.Order.Entities.Order>();
    public DbSet<FoodGrabber.Order.Entities.OrderDetails> OrderDetails => Set<FoodGrabber.Order.Entities.OrderDetails>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MenuEntityConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderEntityConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductEntityConfiguration).Assembly);

        modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
    }
}
