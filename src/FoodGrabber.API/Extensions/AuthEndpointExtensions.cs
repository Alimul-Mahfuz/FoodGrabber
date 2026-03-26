using FoodGrabber.Identity.Entites;
using Microsoft.AspNetCore.Identity;

namespace FoodGrabber.API.Extensions;

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
        UserManager<ApplicationUser> userManager,
        JwtTokenFactory tokenFactory)
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
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            return Results.ValidationProblem(createResult.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
        }

        await userManager.AddToRoleAsync(user, RoleNames.Customer);
        var tokenResult = tokenFactory.CreateToken(user, new[] { RoleNames.Customer });

        return Results.Ok(new AuthResponse(tokenResult.Token, tokenResult.ExpiresAtUtc, user.Email ?? email, new[] { RoleNames.Customer }));
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        UserManager<ApplicationUser> userManager,
        JwtTokenFactory tokenFactory)
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

public sealed record RegisterRequest(string Email, string Password);
public sealed record LoginRequest(string Email, string Password);
public sealed record AuthResponse(string Token, DateTime ExpiresAtUtc, string Email, IEnumerable<string> Roles);
