using FoodGrabber.Product.Abstractions;
using FoodGrabber.Product.Contracts;
using FoodGrabber.Product.Exceptions;
using FoodGrabber.Product.Infrastructure.Persistence;
using FoodGrabber.Product.Infrastructure.Persistence.Repositories;
using FoodGrabber.Product.Services;
using FoodGrabber.Shared.Abstractions;
using FoodGrabber.Shared.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace FoodGrabber.Product.Extensions;

public static class ProductModuleExtensions
{
    public static IServiceCollection AddProductModule(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, EfProductRepository>();
        services.AddScoped<IProductReadContract, ProductReadContract>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductSeedService, ProductSeedService>();
        return services;
    }

    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products");

        group.MapGet("/", GetAllAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("/", CreateAsync).DisableAntiforgery();
        group.MapPut("/{id:guid}", UpdateAsync).DisableAntiforgery();
        group.MapDelete("/{id:guid}", DeleteAsync);

        return app;
    }

    public static async Task SeedProductModuleAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var seedService = scope.ServiceProvider.GetRequiredService<IProductSeedService>();
        await seedService.SeedAsync(cancellationToken);
    }

    private static async Task<IResult> GetAllAsync(
        [AsParameters] PaginationQuery paginationQuery,
        IProductService productService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await productService.GetAllAsync(paginationQuery, cancellationToken);
        result.Items = result.Items.Select(product => product with
        {
            Image = NormalizeImageUrl(product.Image, httpContext)
        });

        return Results.Ok(result);
    }

    private static async Task<IResult> GetByIdAsync(
        Guid id,
        IProductService productService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return Results.NotFound();
        }

        var response = product with
        {
            Image = NormalizeImageUrl(product.Image, httpContext)
        };

        return Results.Ok(response);
    }

    private static async Task<IResult> CreateAsync(
        [FromForm] ProductCreateFormRequest request,
        ClaimsPrincipal user,
        IObjectStorageService objectStorageService,
        IInventoryManagementService inventoryManagementService,
        IProductService productService,
        CancellationToken cancellationToken)
    {
        var userId = ResolveUserId(user);
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        try
        {
            if (request.Image is null || request.Image.Length == 0)
            {
                return Results.BadRequest(new { message = "Image is required." });
            }

            await using var imageStream = request.Image.OpenReadStream();
            var imageUrl = await objectStorageService.UploadAsync(
                imageStream,
                request.Image.FileName,
                request.Image.ContentType,
                cancellationToken);

            var upsertRequest = new ProductUpsertRequest(
                request.Name,
                request.Description,
                request.StockUnit,
                request.BasePrice,
                request.SellingPrice,
                imageUrl,
                request.Tags,
                request.IsActive);

            var createRequest = new ProductCreateRequest(
                upsertRequest.Name,
                upsertRequest.Description,
                request.InitialStock,
                upsertRequest.StockUnit,
                upsertRequest.BasePrice,
                upsertRequest.SellingPrice,
                upsertRequest.Image,
                upsertRequest.Tags,
                upsertRequest.IsActive);

            var created = await productService.CreateAsync(createRequest, userId, cancellationToken);
            await inventoryManagementService.AddInitialStockAsync(
                created.Id,
                request.InitialStock,
                request.StockUnit,
                userId,
                cancellationToken);

            var refreshed = await productService.GetByIdAsync(created.Id, cancellationToken);
            return Results.Created($"/api/products/{created.Id}", refreshed ?? created);
        }
        catch (ProductException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static async Task<IResult> UpdateAsync(
        Guid id,
        [FromForm] ProductUpdateFormRequest request,
        ClaimsPrincipal user,
        IObjectStorageService objectStorageService,
        IInventoryManagementService inventoryManagementService,
        IProductService productService,
        CancellationToken cancellationToken)
    {
        var userId = ResolveUserId(user);
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        try
        {
            var existing = await productService.GetByIdAsync(id, cancellationToken);
            if (existing is null)
            {
                return Results.NotFound();
            }

            var imageUrl = existing.Image;
            var uploadedNewImage = false;

            if (request.Image is { Length: > 0 })
            {
                await using var imageStream = request.Image.OpenReadStream();
                imageUrl = await objectStorageService.UploadAsync(
                    imageStream,
                    request.Image.FileName,
                    request.Image.ContentType,
                    cancellationToken);

                uploadedNewImage = true;
            }

            var upsertRequest = new ProductUpsertRequest(
                request.Name,
                request.Description,
                request.StockUnit,
                request.BasePrice,
                request.SellingPrice,
                imageUrl,
                request.Tags,
                request.IsActive);

            var updated = await productService.UpdateAsync(id, upsertRequest, userId, cancellationToken);
            if (updated is null)
            {
                return Results.NotFound();
            }

            await inventoryManagementService.UpdateStockUnitAsync(id, request.StockUnit, cancellationToken);

            if (uploadedNewImage && !string.IsNullOrWhiteSpace(existing.Image))
            {
                await objectStorageService.DeleteAsync(existing.Image, cancellationToken);
            }

            return Results.Ok(updated);
        }
        catch (ProductException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static async Task<IResult> DeleteAsync(Guid id, IProductService productService, CancellationToken cancellationToken)
    {
        var deleted = await productService.DeleteAsync(id, cancellationToken);
        return deleted ? Results.NoContent() : Results.NotFound();
    }

    private static string? ResolveUserId(ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub");
    }

    private static string? NormalizeImageUrl(string? image, HttpContext httpContext)
    {
        if (string.IsNullOrWhiteSpace(image))
        {
            return image;
        }

        if (Uri.TryCreate(image, UriKind.Absolute, out _))
        {
            return image;
        }

        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        return image.StartsWith('/')
            ? $"{baseUrl}{image}"
            : $"{baseUrl}/{image}";
    }
}
