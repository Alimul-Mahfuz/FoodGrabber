using FoodGrabber.Identity.Abstractions;
using FoodGrabber.Identity.Contracts;
using FoodGrabber.Identity.Exceptions;
using FoodGrabber.Shared.Pagination;

namespace FoodGrabber.Identity.Services
{
    public class UserService(IUserRepository userRepository) : IUserServices
    {
        public async Task<CustomerResponse> CustomerInfoByIdAsync(string guid, CancellationToken ct = default)
        {
            var customer = await userRepository.GetCustomerByIdAsync(guid, ct);

            if (customer == null)
            {
                throw new IdentityException($"Customer with ID {guid} not found");
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

        public async Task<CustomerResponse> UpdateCustomerInfo(CustomerUpdateRequest request, CancellationToken ct = default)
        {

            if (request.FullName == null)
            {
                throw new IdentityException("user name is required");
            }
            if (request.Email == null)
            {
                throw new IdentityException("Email is required");
            }
            throw new NotImplementedException();
        }


    }
}
