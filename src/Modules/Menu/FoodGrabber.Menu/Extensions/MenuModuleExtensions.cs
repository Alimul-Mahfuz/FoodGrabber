using FoodGrabber.Menu.Abstractions;
using FoodGrabber.Menu.Contracts;
using FoodGrabber.Menu.Exceptions;
using FoodGrabber.Menu.Infrastructure.Persistence;
using FoodGrabber.Menu.Infrastructure.Persistence.Repositories;
using FoodGrabber.Menu.Services;
using FoodGrabber.Shared.Pagination;
using FoodGrabber.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;


namespace FoodGrabber.Menu.Extensions;

public static class MenuModuleExtensions
{
    public static IServiceCollection AddMenuModule(this IServiceCollection services)
    {
        services.AddScoped<IMenuRepository, EfMenuRepository>();
        services.AddScoped<IMenuReadContract, MenuReadContract>();
        services.AddScoped<IMenuService, MenuService>();
        return services;
    }

    public static IEndpointRouteBuilder MapMenuEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/menus")
            .WithTags("Menus")
            .RequireAuthorization(new AuthorizeAttribute { Roles = RoleNames.Admin });

        group.MapGet("/", GetAllAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("/", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapDelete("/{id:guid}", DeleteAsync);

        return app;
    }

    private static async Task<IResult> GetAllAsync(
        [AsParameters] PaginationQuery paginationQuery,
        IMenuService menuService,
        CancellationToken cancellationToken)
    {
        var menus = await menuService.GetAllAsync(paginationQuery, cancellationToken);
        return Results.Ok(menus);
    }

    private static async Task<IResult> GetByIdAsync(Guid id, IMenuService menuService, CancellationToken cancellationToken)
    {
        var menu = await menuService.GetByIdAsync(id, cancellationToken);
        return menu is null ? Results.NotFound() : Results.Ok(menu);
    }

    private static async Task<IResult> CreateAsync(MenuUpsertRequest request, IMenuService menuService, CancellationToken cancellationToken)
    {
        try
        {
            var created = await menuService.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/menus/{created.Id}", created);
        }
        catch (MenuException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static async Task<IResult> UpdateAsync(Guid id, MenuUpsertRequest request, IMenuService menuService, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await menuService.UpdateAsync(id, request, cancellationToken);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        }
        catch (MenuException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static async Task<IResult> DeleteAsync(Guid id, IMenuService menuService, CancellationToken cancellationToken)
    {
        var deleted = await menuService.DeleteAsync(id, cancellationToken);
        return deleted ? Results.NoContent() : Results.NotFound();
    }
}
