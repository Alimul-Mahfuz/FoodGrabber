using FoodGrabber.Identity.Abstractions;
using FoodGrabber.Identity.Entites;
using FoodGrabber.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FoodGrabber.Identity.Extensions;

public static class AuthEndpointExtensions
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/api/auth").WithTags("Auth");

        auth.MapPost("/register", RegisterAsync);
        auth.MapPost("/login", LoginAsync);

        return app;
    }

    private static async Task<IResult> RegisterAsync(
        RegisterRequest request,
        [FromServices] UserManager<ApplicationUser> userManager,
        [FromServices] ICustomerProfileStore customerProfileStore,
        [FromServices] JwtTokenFactory tokenFactory,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new { message = "Email and password are required." });
        }

        var email = request.Email.Trim().ToLowerInvariant();
        var existing = await userManager.FindByEmailAsync(email);
        if (existing is not null)
        {
            return Results.Conflict(new { message = "Email is already registered." });
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = request.FullName?.Trim() ?? string.Empty,
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            return Results.ValidationProblem(createResult.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
        }

        await userManager.AddToRoleAsync(user, RoleNames.Customer);

        try
        {
            await customerProfileStore.AddAsync(new Customer
            {
                UserId = user.Id,
                FullName = string.IsNullOrWhiteSpace(request.FullName) ? null : request.FullName.Trim(),
                Email = email,
                IsActive = true
            }, cancellationToken);
        }
        catch
        {
            await userManager.RemoveFromRoleAsync(user, RoleNames.Customer);
            await userManager.DeleteAsync(user);
            return Results.Problem("Unable to create customer profile for this registration.", statusCode: StatusCodes.Status500InternalServerError);
        }

        var tokenResult = tokenFactory.CreateToken(user, new[] { RoleNames.Customer });

        return Results.Ok(new AuthResponse(tokenResult.Token, tokenResult.ExpiresAtUtc, user.Email ?? email, new[] { RoleNames.Customer }));
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        [FromServices] UserManager<ApplicationUser> userManager,
        [FromServices] JwtTokenFactory tokenFactory)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new { message = "Email and password are required." });
        }

        var email = request.Email.Trim().ToLowerInvariant();
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var validPassword = await userManager.CheckPasswordAsync(user, request.Password);
        if (!validPassword)
        {
            return Results.Unauthorized();
        }

        var roles = await userManager.GetRolesAsync(user);
        var tokenResult = tokenFactory.CreateToken(user, roles);

        return Results.Ok(new AuthResponse(tokenResult.Token, tokenResult.ExpiresAtUtc, user.Email ?? email, roles));
    }
}



public sealed record RegisterRequest(string Email, string Password, string? FullName);
public sealed record LoginRequest(string Email, string Password);
public sealed record AuthResponse(string Token, DateTime ExpiresAtUtc, string Email, IEnumerable<string> Roles);
