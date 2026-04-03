using FoodGrabber.Identity.Abstractions;
using FoodGrabber.Identity.Contracts;
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


            user.MapGet("/customer/{Id}", GetCustomerInfo).RequireAuthorization(new AuthorizeAttribute
            {
                Roles = $"{RoleNames.Admin}"
            });
            user.MapGet("/", GetUserList).RequireAuthorization(new AuthorizeAttribute { Roles = RoleNames.Admin });
            user.MapPut("/updateCustomer", UpdateCustomerInfo).RequireAuthorization(new AuthorizeAttribute { Roles = RoleNames.Admin });



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
            [FromQuery] string customerId,
            [FromServices] IUserServices userServices,
            ClaimsPrincipal principal,
            CancellationToken cancellationToken)
        {
            var customer = await userServices.CustomerInfoByIdAsync(customerId);
            return Results.Ok(customer);
        }

        private static async Task<IResult> UpdateCustomerInfo(CustomerUpdateRequest customerUpdateRequest,
            ClaimsPrincipal user,
            [FromServices] IUserServices userServices,
            CancellationToken cancellationToken = default)
        {
            var response = await userServices.UpdateCustomerInfo(customerUpdateRequest, cancellationToken);
            return Results.Ok(response);
        }

        private static string? ResolveUserId(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue("sub");
        }


    }
}
