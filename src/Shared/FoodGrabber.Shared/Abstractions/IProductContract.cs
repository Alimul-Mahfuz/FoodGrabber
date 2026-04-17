namespace FoodGrabber.Shared.Abstractions
{

    public sealed record ProductPriceResponse(string Id, string Name, decimal SellingPrice);
    public interface IProductContract
    {
        Task<List<ProductPriceResponse>> GetThePriceById(string[] ProductId, CancellationToken ctx);
    }
}
