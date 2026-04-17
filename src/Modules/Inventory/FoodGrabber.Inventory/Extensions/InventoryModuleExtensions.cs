using FoodGrabber.Inventory.Abstractions;
using FoodGrabber.Inventory.Contracts;
using FoodGrabber.Inventory.Exceptions;
using FoodGrabber.Inventory.Infrastructure.Persistence.Repositories;
using FoodGrabber.Inventory.Services;
using FoodGrabber.Shared.Abstractions;
using FoodGrabber.Shared.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace FoodGrabber.Inventory.Extensions;

public static class InventoryModuleExtensions
{
    public static IServiceCollection AddInventoryModule(this IServiceCollection services)
    {
        services.AddScoped<IInventoryRepository, EfInventoryRepository>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IInventoryManagementService, InventoryManagementService>();
        return services;
    }

    public static IEndpointRouteBuilder MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory")
            .WithTags("Inventory");
        //.RequireAuthorization(new AuthorizeAttribute { Roles = RoleNames.Admin });

        group.MapGet("/products/{productId:guid}/entries", GetEntriesAsync);
        group.MapPost("/products/{productId:guid}/adjust", AdjustStockAsync);

        return app;
    }

    private static async Task<IResult> GetEntriesAsync(
        Guid productId,
        [AsParameters] PaginationQuery paginationQuery,
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        return Results.Ok(await inventoryService.GetEntriesAsync(productId, paginationQuery, cancellationToken));
    }

    private static async Task<IResult> AdjustStockAsync(
        Guid productId,
        StockAdjustmentRequest request,
        ClaimsPrincipal user,
        IInventoryService inventoryService,
        CancellationToken cancellationToken)
    {
        var userId = ResolveUserId(user);
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        try
        {
            var result = await inventoryService.AdjustStockAsync(productId, request, userId, cancellationToken);
            return Results.Ok(result);
        }
        catch (InventoryException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static string? ResolveUserId(ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub");
    }
}
