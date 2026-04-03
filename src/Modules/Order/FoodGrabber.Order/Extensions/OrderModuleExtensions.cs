using FoodGrabber.Order.Abstractions;
using FoodGrabber.Order.Contracts;
using FoodGrabber.Order.Exceptions;
using FoodGrabber.Order.Infrastructure.Persistence.Repositories;
using FoodGrabber.Order.Services;
using FoodGrabber.Shared.Pagination;
using FoodGrabber.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace FoodGrabber.Order.Extensions;

public static class OrderModuleExtensions
{
    public static IServiceCollection AddOrderModule(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        services.AddScoped<IOrderService, OrderServices>();
        return services;
    }


    public static IEndpointRouteBuilder MapOrderEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders")
            .WithTags("Orders")
            .RequireAuthorization(new AuthorizeAttribute
            {
                Roles = $"{RoleNames.Admin},{RoleNames.Customer}"
            });

        group.MapGet("/", GetAllAsync);
        group.MapPost("/", CreateAsync);

        return app;
    }

    private static async Task<IResult> GetAllAsync(
        [AsParameters] PaginationQuery paginationQuery,
        IOrderService orderService,
        CancellationToken cancellationToken)
    {
        var order = await orderService.GetAllAsync(paginationQuery, cancellationToken);
        return Results.Ok(order);
    }

    private static async Task<IResult> CreateAsync(OrderUpsertRequest request, IOrderService orderService, CancellationToken cancellationToken)
    {
        try
        {
            var created = await orderService.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/orders/{created.Id}", created);
        }
        catch (OrderModuleException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }


}


