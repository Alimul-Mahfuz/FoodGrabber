namespace FoodGrabber.Product.Abstractions;

public interface IProductSeedService
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
