using FoodGrabber.Identity.Abstractions;
using FoodGrabber.Identity.Contracts;
using FoodGrabber.Identity.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodGrabber.Identity.Infrastructures
{
    public class UserRepositories(DbContext dbContext) : IUserRepository
    {
        public Task AddAsync(ApplicationUser entity, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CountAsync(CancellationToken ct = default)
        {
            return await dbContext.Set<ApplicationUser>().CountAsync(ct);
        }

        public Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ApplicationUser>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer?> GetCustomerByIdAsync(string id, CancellationToken ct = default)
        {
            Guid.TryParse(id, out var customerId);
            var customer = await dbContext.Set<Customer>().FirstOrDefaultAsync(c => c.Id == customerId, ct);
            return customer;
        }

        public Task<IReadOnlyList<ApplicationUser>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<UserWithRoleDto>> GetPagedWithRolesAsync(
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var users = dbContext.Set<ApplicationUser>();
            var userRoles = dbContext.Set<IdentityUserRole<string>>();
            var roles = dbContext.Set<ApplicationRole>();

            var query =
                from user in users
                join userRole in userRoles on user.Id equals userRole.UserId
                join role in roles on userRole.RoleId equals role.Id
                orderby user.Id descending
                select new UserWithRoleDto(
                    user.Id,
                    user.FullName,
                    user.Email!,
                    role.Name!
                );

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public Task UpdateAsync(ApplicationUser entity, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCustomerInfoByIdAsync(string id, Customer customerEntity, CancellationToken ct)
        {
            dbContext.Set<Customer>().Update(customerEntity);
            await dbContext.SaveChangesAsync(ct);

        }
    }
}
