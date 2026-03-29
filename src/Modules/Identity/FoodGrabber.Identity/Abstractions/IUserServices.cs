using FoodGrabber.Identity.Contracts;
using FoodGrabber.Shared.Pagination;

namespace FoodGrabber.Identity.Abstractions
{
    public interface IUserServices
    {
        public Task<CustomerResponse> CustomerInfoByIdAsync(string guid, CancellationToken ct = default);
        public Task<PagedResult<UserResponse>> PagedUserListAsync(PaginationQuery paginationQuery, CancellationToken ct = default);

        public Task<CustomerResponse> UpdateCustomerInfo(CustomerUpdateRequest request, CancellationToken ct = default);
    }
}
