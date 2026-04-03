using FoodGrabber.Product.Abstractions;
using FoodGrabber.Product.Contracts;
using FoodGrabber.Product.Exceptions;
using FoodGrabber.Shared.Pagination;
using ProductEntity = FoodGrabber.Product.Entities.Product;

namespace FoodGrabber.Product.Services;

public sealed class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<PagedResult<ProductResponse>> GetAllAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        var page = paginationQuery.NormalizedPage;
        var pageSize = paginationQuery.NormalizedPageSize;
        var products = await productRepository.GetPagedAsync(page, pageSize, cancellationToken);
        var totalCount = await productRepository.CountAsync(cancellationToken);
        var items = products.Select(MapToResponse).ToList();
        return new PagedResult<ProductResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken);
        return product is null ? null : MapToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(ProductCreateRequest request, string userId, CancellationToken cancellationToken = default)
    {
        ValidateCreateRequest(request);

        var normalizedUnit = NormalizeStockUnit(request.StockUnit);

        var product = new ProductEntity
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            CurrentStock = 0,
            StockUnit = normalizedUnit,
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
        product.StockUnit = NormalizeStockUnit(request.StockUnit);
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

        if (string.IsNullOrWhiteSpace(request.StockUnit))
        {
            throw new ProductException("Stock unit is required.");
        }

        if (request.BasePrice < 0 || request.SellingPrice < 0)
        {
            throw new ProductException("Prices must be non-negative.");
        }
    }

    private static void ValidateCreateRequest(ProductCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Description))
        {
            throw new ProductException("Name and description are required.");
        }

        if (string.IsNullOrWhiteSpace(request.StockUnit))
        {
            throw new ProductException("Stock unit is required.");
        }

        if (request.InitialStock < 0 || request.BasePrice < 0 || request.SellingPrice < 0)
        {
            throw new ProductException("Initial stock and prices must be non-negative.");
        }
    }

    private static string NormalizeStockUnit(string stockUnit)
    {
        var normalized = stockUnit.Trim().ToLowerInvariant();
        return normalized;
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
            product.CurrentStock,
            product.StockUnit,
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
