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
                cart.TotalPrice += incomingItem.Quantity * incomingItem.UnitPrice;
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
                    UnitPrice = incomingItem.UnitPrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else
            {
                existingItem.Quantity += incomingItem.Quantity;
                existingItem.UnitPrice = incomingItem.UnitPrice;
                existingItem.UpdatedAt = DateTime.UtcNow;
            }

            existingCart.TotalPrice += incomingItem.Quantity * incomingItem.UnitPrice;
        }

        existingCart.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(ct);
        return existingCart;
    }

    public async Task<CartEntity?> IsItemExistOnActiveCart(Guid itemId, Guid userId)
    {
        var cart = await dbContext.Set<CartEntity>()
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId
                                   && c.Status != CartStatus.Complete);

        if (cart is null)
            return null;

        return cart.CartItems.Any(item => item.ItemId == itemId) ? cart : null;
    }

    public async Task<bool> RemoveItemFromCartAsync(Guid ItemId, Guid UserId, int Quantity, CancellationToken ctx = default)
    {
        var cart = await this.IsItemExistOnActiveCart(ItemId, UserId);
        if (cart == null)
        {
            return false;
        }

        var item = cart.CartItems.FirstOrDefault(i => i.ItemId == ItemId);

        if (item == null)
        {
            return false;
        }

        if (Quantity >= item.Quantity)
        {
            cart.CartItems.Remove(item);
        }
        else
        {
            item.Quantity -= Quantity;
            item.UpdatedAt = DateTime.UtcNow;
        }
        cart.TotalPrice = cart.CartItems.Sum(i => i.Quantity * i.UnitPrice);
        cart.UpdatedAt = DateTime.UtcNow;

        if (cart.CartItems.Count == 0)
        {
            dbContext.Remove(cart);
        }

        await dbContext.SaveChangesAsync();

        return true;


    }

    public async Task<CartEntity?> ShowCart(Guid UserId, CancellationToken ctx = default)
    {
        return await dbContext.Set<CartEntity>()
            .Include(cart => cart.CartItems)
            .FirstOrDefaultAsync(cart => cart.UserId == UserId);
    }
}
