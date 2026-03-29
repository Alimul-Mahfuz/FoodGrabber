using FoodGrabber.Identity.Abstractions;
using FoodGrabber.Shared.Pagination;
using FoodGrabber.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace FoodGrabber.Identity.Extensions
{
    public static class UserEndpointExtensions
    {

        public static IEndpointRouteBuilder MapUserEndPoint(this IEndpointRouteBuilder app)
        {
            var user = app.MapGroup("/api/user")
                .WithTags("User");


            user.MapGet("/me", GetCustomerInfo).RequireAuthorization(new AuthorizeAttribute { Roles = $"{RoleNames.Admin},{RoleNames.Customer}" });
            user.MapGet("/", GetUserList).RequireAuthorization(new AuthorizeAttribute { Roles = RoleNames.Admin });
            //user.MapPut("/updateCustomer",)



            return user;
        }

        private static async Task<IResult> GetUserList(
            [AsParameters] PaginationQuery paginationQuery,
            [FromServices] IUserServices userServices,
            CancellationToken ct
            )
        {
            var data = await userServices.PagedUserListAsync(paginationQuery, ct);
            return Results.Ok(data);
        }

        private static async Task<IResult> GetCustomerInfo(
            [FromServices] IUserServices userServices,
            ClaimsPrincipal principal,
            CancellationToken cancellationToken)
        {
            var userId = ResolveUserId(principal);
            if (userId == null)
            {
                return Results.BadRequest("User not found");
            }
            var customer = await userServices.CustomerInfoByIdAsync(userId);


            return Results.Ok(customer);
        }

        private static string? ResolveUserId(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue("sub");
        }


    }
}
