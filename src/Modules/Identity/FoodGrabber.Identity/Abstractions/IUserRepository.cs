using FoodGrabber.Identity.Contracts;
using FoodGrabber.Identity.Entites;
using FoodGrabber.Shared.Abstractions;
using CustomerEntity = FoodGrabber.Identity.Entites.Customer;

namespace FoodGrabber.Identity.Abstractions
{
    public interface IUserRepository : IRepository<ApplicationUser, Guid>
    {
        Task<Customer?> GetCustomerByIdAsync(string id, CancellationToken ct = default);
        Task<IReadOnlyList<UserWithRoleDto>> GetPagedWithRolesAsync(
            int page,
            int pageSize,
            CancellationToken ct = default
            );

        Task UpdateCustomerInfoByIdAsync(string id, CustomerEntity customerEntity, CancellationToken ct);
    }
}
