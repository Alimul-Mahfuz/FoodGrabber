using FoodGrabber.Product.Abstractions;
using FoodGrabber.Shared.Abstractions;
using ProductEntity = FoodGrabber.Product.Entities.Product;

namespace FoodGrabber.Product.Services;

public sealed class ProductSeedService(
    IProductRepository productRepository,
    IAdminUserProvider adminUserProvider) : IProductSeedService
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await productRepository.EnsureStorageAsync(cancellationToken);
        if (await productRepository.AnyAsync(cancellationToken))
        {
            return;
        }

        var adminUserId = await adminUserProvider.GetDefaultAdminUserIdAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(adminUserId))
        {
            throw new InvalidOperationException("Default admin user was not found for product seeding.");
        }

        var now = DateTime.UtcNow;
        var products = new[]
        {
            new ProductEntity
            {
                Name = "Cheese Burger",
                Description = "Classic grilled burger with cheese.",
                Quantity = 120,
                BasePrice = 4.50m,
                SellingPrice = 6.99m,
                Tags = "burger,fast-food,beef",
                IsActive = true,
                UserId = adminUserId,
                CreatedAt = now,
                UpdatedAt = now
            },
            new ProductEntity
            {
                Name = "Chicken Pizza",
                Description = "Thin crust pizza topped with chicken and mozzarella.",
                Quantity = 80,
                BasePrice = 7.00m,
                SellingPrice = 10.50m,
                Tags = "pizza,chicken,dinner",
                IsActive = true,
                UserId = adminUserId,
                CreatedAt = now,
                UpdatedAt = now
            },
            new ProductEntity
            {
                Name = "Cold Coffee",
                Description = "Chilled coffee drink with creamy texture.",
                Quantity = 200,
                BasePrice = 1.20m,
                SellingPrice = 2.99m,
                Tags = "drink,coffee,cold",
                IsActive = true,
                UserId = adminUserId,
                CreatedAt = now,
                UpdatedAt = now
            }
        };

        foreach (var product in products)
        {
            await productRepository.AddAsync(product, cancellationToken);
        }
    }
}
