using FoodGrabber.Identity.Entites;

namespace FoodGrabber.Identity.Abstractions;

public interface ICustomerProfileStore
{
    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<Customer?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
