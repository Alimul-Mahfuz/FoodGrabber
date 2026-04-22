using FoodGrabber.Cart.Abstractions;
using FoodGrabber.Cart.Contracts;
using FoodGrabber.Cart.Infrastructure.Persistence.Repositories;
using FoodGrabber.Cart.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace FoodGrabber.Cart.Extensions;

public static class CartModuleExtensions
{
    public static IServiceCollection AddCartModule(this IServiceCollection services)
    {
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICartRepository, EfCartRepository>();
        return services;
    }

    public static IEndpointRouteBuilder MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/cart")
            .WithTags("Cart");

        group.MapPost("add", AddToCart);
        group.MapPost("remove", RemoveItemFromCart);
        group.MapGet("myCart", ViewCart);


        return app;
    }

    private static async Task<IResult> AddToCart(
        [FromBody] AddToCartRequest addToCartRequest,
        [FromServices] ICartService cartService,
        ClaimsPrincipal user,
        CancellationToken ctx)
    {
        if (user.Identity?.IsAuthenticated != true)
        {
            return Results.Unauthorized();
        }

        var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub");

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Results.Unauthorized();
        }

        try
        {
            var response = await cartService.CreateCartAsync(addToCartRequest, userId, ctx);
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static async Task<IResult> RemoveItemFromCart(
        [FromBody] RemoveItemFromCartRequest removeItemFromCartRequest,
        [FromServices] ICartService cart,
        CancellationToken ctx,
        ClaimsPrincipal user
        )
    {
        if (user.Identity?.IsAuthenticated != true)
        {
            return Results.Unauthorized();
        }

        var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub");

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Results.Unauthorized();
        }

        try
        {
            var response = await cart.RemoveItemFromCart(removeItemFromCartRequest, userIdValue, ctx);
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);

        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private static async Task<IResult> ViewCart(
        [FromServices] ICartService cartService,
        ClaimsPrincipal user,
        CancellationToken cancellationToken
        )
    {
        if (user.Identity?.IsAuthenticated != true)
        {
            return Results.Unauthorized();
        }

        var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub");

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Results.Unauthorized();
        }

        try
        {
            var response = await cartService.ViewMyCart(userId, cancellationToken);
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }

    }
}
