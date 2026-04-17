using FoodGrabber.Identity.Entites;
using FoodGrabber.Infrastructure.Data.Configurations;
using FoodGrabber.Cart.Infrastructure.Persistence.Configurations;
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
    public DbSet<FoodGrabber.Cart.Entities.Cart> Carts => Set<FoodGrabber.Cart.Entities.Cart>();
    public DbSet<FoodGrabber.Cart.Entities.CartItem> CartItems => Set<FoodGrabber.Cart.Entities.CartItem>();
    public DbSet<FoodGrabber.Identity.Entites.Customer> Customers => Set<Customer>();
    public DbSet<FoodGrabber.Menu.Entities.Menu> Menus => Set<FoodGrabber.Menu.Entities.Menu>();
    public DbSet<FoodGrabber.Menu.Entities.MenuProduct> MenuProducts => Set<FoodGrabber.Menu.Entities.MenuProduct>();
    public DbSet<FoodGrabber.Menu.Entities.MenuCategory> MenuCategories => Set<FoodGrabber.Menu.Entities.MenuCategory>();
    public DbSet<FoodGrabber.Menu.Entities.MenuCategoryProduct> MenuCategoryProducts => Set<FoodGrabber.Menu.Entities.MenuCategoryProduct>();
    public DbSet<FoodGrabber.Product.Entities.Product> Products => Set<FoodGrabber.Product.Entities.Product>();
    public DbSet<FoodGrabber.Product.Entities.ProductStockEntry> ProductStockEntries => Set<FoodGrabber.Product.Entities.ProductStockEntry>();
    public DbSet<FoodGrabber.Product.Entities.ProductCategory> ProductCategories => Set<FoodGrabber.Product.Entities.ProductCategory>();
    public DbSet<FoodGrabber.Product.Entities.ProductModifierGroup> ProductModifierGroups => Set<FoodGrabber.Product.Entities.ProductModifierGroup>();
    public DbSet<FoodGrabber.Product.Entities.ProductModifierOption> ProductModifierOptions => Set<FoodGrabber.Product.Entities.ProductModifierOption>();
    public DbSet<FoodGrabber.Product.Entities.ProductPriceHistory> ProductPriceHistories => Set<FoodGrabber.Product.Entities.ProductPriceHistory>();
    public DbSet<FoodGrabber.Product.Entities.ProductAvailabilityWindow> ProductAvailabilityWindows => Set<FoodGrabber.Product.Entities.ProductAvailabilityWindow>();
    public DbSet<FoodGrabber.Order.Entities.Order> Orders => Set<FoodGrabber.Order.Entities.Order>();
    public DbSet<FoodGrabber.Order.Entities.OrderDetails> OrderDetails => Set<FoodGrabber.Order.Entities.OrderDetails>();
    public DbSet<FoodGrabber.Order.Entities.OrderStatusHistory> OrderStatusHistory => Set<FoodGrabber.Order.Entities.OrderStatusHistory>();
    public DbSet<FoodGrabber.Order.Entities.OrderItemModifier> OrderItemModifiers => Set<FoodGrabber.Order.Entities.OrderItemModifier>();
    public DbSet<FoodGrabber.Order.Entities.Restaurant> Restaurants => Set<FoodGrabber.Order.Entities.Restaurant>();
    public DbSet<FoodGrabber.Order.Entities.Branch> Branches => Set<FoodGrabber.Order.Entities.Branch>();
    public DbSet<FoodGrabber.Order.Entities.DiningTable> DiningTables => Set<FoodGrabber.Order.Entities.DiningTable>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerEntityConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartEntityConfiguration).Assembly);
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
