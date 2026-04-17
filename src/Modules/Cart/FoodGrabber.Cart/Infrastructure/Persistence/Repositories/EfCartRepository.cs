using FoodGrabber.Cart.Abstractions;
using FoodGrabber.Cart.Entities;
using Microsoft.EntityFrameworkCore;
using CartEntity = FoodGrabber.Cart.Entities.Cart;

namespace FoodGrabber.Cart.Infrastructure.Persistence.Repositories;

public sealed class EfCartRepository(DbContext dbContext) : ICartRepository
{
    public Task<CartEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return dbContext.Set<CartEntity>()
            .AsNoTracking()
            .Include(cart => cart.CartItems)
            .FirstOrDefaultAsync(cart => cart.Id == id, ct);
    }

    public async Task<IReadOnlyList<CartEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await dbContext.Set<CartEntity>()
            .AsNoTracking()
            .Include(cart => cart.CartItems)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<CartEntity>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await dbContext.Set<CartEntity>()
            .AsNoTracking()
            .Include(cart => cart.CartItems)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(CancellationToken ct = default)
    {
        return dbContext.Set<CartEntity>().CountAsync(ct);
    }

    public async Task AddAsync(CartEntity entity, CancellationToken ct = default)
    {
        dbContext.Set<CartEntity>().Add(entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(CartEntity entity, CancellationToken ct = default)
    {
        dbContext.Set<CartEntity>().Update(entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var cart = await dbContext.Set<CartEntity>().FirstOrDefaultAsync(currentCart => currentCart.Id == id, ct);
        if (cart is null)
        {
            return;
        }

        dbContext.Set<CartEntity>().Remove(cart);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<CartEntity> AddTheItemToCartAsync(CartEntity cart, CancellationToken ct = default)
    {
        var existingCart = await dbContext.Set<CartEntity>()
            .Include(currentCart => currentCart.CartItems)
            .FirstOrDefaultAsync(
                cartUser => cartUser.UserId == cart.UserId && cartUser.Status != CartStatus.Complete,
                ct);

        if (existingCart is null)
        {
            cart.TotalPrice = 0m;
            foreach (var incomingItem in cart.CartItems)
            {
                cart.TotalPrice += incomingItem.Quantity;
            }

            dbContext.Set<CartEntity>().Add(cart);
            await dbContext.SaveChangesAsync(ct);
            return cart;
        }

        foreach (var incomingItem in cart.CartItems)
        {
            var existingItem = existingCart.CartItems.FirstOrDefault(currentItem =>
                currentItem.ItemId == incomingItem.ItemId &&
                currentItem.ItemType == incomingItem.ItemType);

            if (existingItem is null)
            {
                existingCart.CartItems.Add(new CartItem
                {
                    CartId = existingCart.Id,
                    ItemId = incomingItem.ItemId,
                    ItemType = incomingItem.ItemType,
                    Quantity = incomingItem.Quantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else
            {
                existingItem.Quantity += incomingItem.Quantity;
                existingItem.UpdatedAt = DateTime.UtcNow;
            }

            existingCart.TotalPrice += incomingItem.Quantity;
        }

        existingCart.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(ct);
        return existingCart;
    }
}
