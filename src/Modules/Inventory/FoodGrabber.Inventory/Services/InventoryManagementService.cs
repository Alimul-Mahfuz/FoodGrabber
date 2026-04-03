using FoodGrabber.Inventory.Abstractions;
using FoodGrabber.Inventory.Exceptions;
using FoodGrabber.Product.Entities;
using FoodGrabber.Shared.Abstractions;

namespace FoodGrabber.Inventory.Services;

public sealed class InventoryManagementService(IInventoryRepository inventoryRepository) : IInventoryManagementService
{
    public async Task AddInitialStockAsync(
        Guid productId,
        decimal initialStock,
        string stockUnit,
        string userId,
        CancellationToken cancellationToken = default)
    {
        if (initialStock < 0)
        {
            throw new InventoryException("Initial stock must be non-negative.");
        }

        if (string.IsNullOrWhiteSpace(stockUnit))
        {
            throw new InventoryException("Stock unit is required.");
        }

        var product = await inventoryRepository.GetProductAsync(productId, cancellationToken);
        if (product is null)
        {
            throw new InventoryException("Product not found.");
        }

        var normalizedUnit = stockUnit.Trim().ToLowerInvariant();
        product.StockUnit = normalizedUnit;
        product.CurrentStock = initialStock;
        product.UpdatedAt = DateTime.UtcNow;

        if (initialStock > 0)
        {
            var entry = new ProductStockEntry
            {
                ProductId = productId,
                Quantity = initialStock,
                MovementType = "initial",
                Unit = normalizedUnit,
                Notes = "Initial stock on product creation.",
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await inventoryRepository.AddStockEntryAsync(entry, cancellationToken);
        }

        await inventoryRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateStockUnitAsync(
        Guid productId,
        string stockUnit,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(stockUnit))
        {
            throw new InventoryException("Stock unit is required.");
        }

        var product = await inventoryRepository.GetProductAsync(productId, cancellationToken);
        if (product is null)
        {
            throw new InventoryException("Product not found.");
        }

        product.StockUnit = stockUnit.Trim().ToLowerInvariant();
        product.UpdatedAt = DateTime.UtcNow;
        await inventoryRepository.SaveChangesAsync(cancellationToken);
    }
}
