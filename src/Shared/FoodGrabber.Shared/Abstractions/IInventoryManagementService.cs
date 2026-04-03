namespace FoodGrabber.Shared.Abstractions;

public interface IInventoryManagementService
{
    Task AddInitialStockAsync(
        Guid productId,
        decimal initialStock,
        string stockUnit,
        string userId,
        CancellationToken cancellationToken = default);

    Task UpdateStockUnitAsync(
        Guid productId,
        string stockUnit,
        CancellationToken cancellationToken = default);
}
