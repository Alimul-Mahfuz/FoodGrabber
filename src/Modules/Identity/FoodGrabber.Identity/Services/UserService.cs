using FoodGrabber.Identity.Abstractions;
using FoodGrabber.Identity.Contracts;
using FoodGrabber.Identity.Entites;
using FoodGrabber.Identity.Exceptions;
using FoodGrabber.Shared.Pagination;
using Microsoft.AspNetCore.Identity;

namespace FoodGrabber.Identity.Services
{
    public class UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager) : IUserServices
    {
        public async Task<CustomerResponse> CustomerInfoByIdAsync(string customerId, CancellationToken ct = default)
        {
            var customer = await userRepository.GetCustomerByIdAsync(customerId, ct);

            if (customer == null)
            {
                throw new IdentityException($"Customer with ID {customerId} not found");
            }

            return new CustomerResponse(
                Id: customer.Id,
                UserId: customer.UserId,
                FullName: customer.FullName ?? string.Empty,
                Email: customer.Email ?? string.Empty,
                Address1: customer.Address1 ?? string.Empty,
                Address2: customer.Address2 ?? string.Empty,
                Phone1: customer.Phone1 ?? string.Empty,
                Phone2: customer.Phone2 ?? string.Empty,
                Image: customer.Image ?? string.Empty
            );
        }

        public async Task<PagedResult<UserResponse>> PagedUserListAsync(PaginationQuery paginationQuery, CancellationToken ct = default)
        {
            var page = paginationQuery.NormalizedPage;
            var pageSize = paginationQuery.NormalizedPageSize;
            var recordCount = await userRepository.CountAsync(ct);
            var records = await userRepository.GetPagedWithRolesAsync(page, pageSize, ct);
            var users = records.Select(dt =>
            {
                Guid.TryParse(dt.Id, out var guid);

                return new UserResponse(
                    guid,
                    dt.FullName ?? string.Empty,
                    dt.Email,
                    dt.Role ?? string.Empty
                );
            }).ToList();
            return new PagedResult<UserResponse>
            {
                Items = users,
                TotalCount = records.Count,
                Page = page,
                PageSize = pageSize

            };
        }

        public async Task<CustomerResponse> UpdateCustomerInfo(
            CustomerUpdateRequest request,
            CancellationToken ct = default)
        {
            if (request.Id == null)
            {
                throw new IdentityException("Please provide customer id");
            }

            if (request.FullName == null)
            {
                throw new IdentityException("user name is required");
            }
            if (request.Email == null)
            {
                throw new IdentityException("Email is required");
            }

            var customer = await userRepository.GetCustomerByIdAsync(request.Id, ct);
            if (customer is null)
            {
                throw new IdentityException("Customer Id invalid");
            }
            customer.Phone1 = request.Phone1;
            customer.Address1 = request.Address1;
            customer.Address2 = request.Address2;
            customer.Email = request.Email;
            customer.FullName = request.FullName;

            var user = await userManager.FindByIdAsync(customer.UserId);
            if (user is null)
            {
                throw new IdentityException("User not found");
            }

            user.Email = request.Email;
            user.FullName = request.FullName;
            await userManager.UpdateAsync(user);
            await userRepository.UpdateCustomerInfoByIdAsync(request.Id, customer, ct);

            return new CustomerResponse
            (
                customer.Id,
                customer.UserId,
                customer.FullName,
                customer.Phone1,
                customer.Phone2 ?? "",
                customer.Address1,
                customer.Address2,
                customer.Email,
                customer.Image ?? ""
            );
        }


    }
}
