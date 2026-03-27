using FoodGrabber.Product.Abstractions;
using FoodGrabber.Product.Contracts;
using FoodGrabber.Product.Exceptions;
using ProductEntity = FoodGrabber.Product.Entities.Product;

namespace FoodGrabber.Product.Services;

public sealed class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);
        return products.Select(MapToResponse).OrderByDescending(p => p.CreatedAt).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken);
        return product is null ? null : MapToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(ProductUpsertRequest request, string userId, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var product = new ProductEntity
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Quantity = request.Quantity,
            BasePrice = request.BasePrice,
            SellingPrice = request.SellingPrice,
            Image = string.IsNullOrWhiteSpace(request.Image) ? null : request.Image.Trim(),
            Tags = NormalizeTags(request.Tags),
            IsActive = request.IsActive,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await productRepository.AddAsync(product, cancellationToken);
        return MapToResponse(product);
    }

    public async Task<ProductResponse?> UpdateAsync(Guid id, ProductUpsertRequest request, string userId, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var product = await productRepository.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return null;
        }

        product.Name = request.Name.Trim();
        product.Description = request.Description.Trim();
        product.Quantity = request.Quantity;
        product.BasePrice = request.BasePrice;
        product.SellingPrice = request.SellingPrice;
        product.Image = string.IsNullOrWhiteSpace(request.Image) ? null : request.Image.Trim();
        product.Tags = NormalizeTags(request.Tags);
        product.IsActive = request.IsActive;
        product.UserId = userId;
        product.UpdatedAt = DateTime.UtcNow;

        await productRepository.UpdateAsync(product, cancellationToken);
        return MapToResponse(product);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return false;
        }

        await productRepository.DeleteAsync(product, cancellationToken);
        return true;
    }

    private static void ValidateRequest(ProductUpsertRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Description))
        {
            throw new ProductException("Name and description are required.");
        }

        if (request.Quantity < 0 || request.BasePrice < 0 || request.SellingPrice < 0)
        {
            throw new ProductException("Quantity and prices must be non-negative.");
        }
    }

    private static string NormalizeTags(IEnumerable<string>? tags)
    {
        return string.Join(",", tags?.Select(t => t.Trim()).Where(t => !string.IsNullOrWhiteSpace(t)) ?? []);
    }

    private static ProductResponse MapToResponse(ProductEntity product)
    {
        var tags = string.IsNullOrWhiteSpace(product.Tags)
            ? []
            : product.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Quantity,
            product.BasePrice,
            product.SellingPrice,
            product.Image,
            tags,
            product.IsActive,
            product.UserId,
            product.CreatedAt,
            product.UpdatedAt);
    }
}
