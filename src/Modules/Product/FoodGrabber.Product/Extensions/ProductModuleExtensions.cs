using FoodGrabber.Product.Abstractions;
using FoodGrabber.Product.Contracts;
using FoodGrabber.Product.Exceptions;
using FoodGrabber.Product.Infrastructure.Persistence.Repositories;
using FoodGrabber.Product.Services;
using FoodGrabber.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace FoodGrabber.Product.Extensions;

public static class ProductModuleExtensions
{
    public static IServiceCollection AddProductModule(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, EfProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductSeedService, ProductSeedService>();
        return services;
    }

    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products")
            .RequireAuthorization(new AuthorizeAttribute { Roles = RoleNames.Admin });

        group.MapGet("/", GetAllAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("/", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);

        return app;
    }

    public static async Task SeedProductModuleAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var seedService = scope.ServiceProvider.GetRequiredService<IProductSeedService>();
        await seedService.SeedAsync(cancellationToken);
    }

    private static async Task<IResult> GetAllAsync(IProductService productService, CancellationToken cancellationToken)
    {
        return Results.Ok(await productService.GetAllAsync(cancellationToken));
    }

    private static async Task<IResult> GetByIdAsync(Guid id, IProductService productService, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(id, cancellationToken);
        return product is null ? Results.NotFound() : Results.Ok(product);
    }

    private static async Task<IResult> CreateAsync(
        ProductUpsertRequest request,
        ClaimsPrincipal user,
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
            var created = await productService.CreateAsync(request, userId, cancellationToken);
            return Results.Created($"/api/products/{created.Id}", created);
        }
        catch (ProductException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static async Task<IResult> UpdateAsync(
        Guid id,
        ProductUpsertRequest request,
        ClaimsPrincipal user,
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
            var updated = await productService.UpdateAsync(id, request, userId, cancellationToken);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
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
}
