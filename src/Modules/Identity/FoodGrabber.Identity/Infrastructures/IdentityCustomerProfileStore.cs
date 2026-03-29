using FoodGrabber.Identity.Abstractions;
using FoodGrabber.Identity.Entites;
using Microsoft.EntityFrameworkCore;

namespace FoodGrabber.Identity.Infrastructures;

public sealed class IdentityCustomerProfileStore(DbContext dbContext) : ICustomerProfileStore
{
    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        dbContext.Set<Customer>().Add(customer);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Customer?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<Customer>().FirstOrDefaultAsync(customer => customer.UserId == userId, cancellationToken);
    }
}
