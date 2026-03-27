using FoodGrabber.Menu.Abstractions;
using FoodGrabber.Menu.Contracts;
using FoodGrabber.Menu.Entities;
using FoodGrabber.Menu.Exceptions;
using MenuEntity = FoodGrabber.Menu.Entities.Menu;

namespace FoodGrabber.Menu.Services;

public sealed class MenuService(IMenuRepository menuRepository) : IMenuService
{
    public async Task<IReadOnlyList<MenuResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var menus = await menuRepository.GetAllAsync(cancellationToken);
        return menus.Select(MapToResponse).OrderByDescending(m => m.CreatedAt).ToList();
    }

    public async Task<MenuResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var menu = await menuRepository.GetByIdAsync(id, cancellationToken);
        return menu is null ? null : MapToResponse(menu);
    }

    public async Task<MenuResponse> CreateAsync(MenuUpsertRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var now = DateTime.UtcNow;
        var menu = new MenuEntity
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            SellingPrice = request.SellingPrice,
            IsActive = request.IsActive,
            CreatedAt = now,
            UpdatedAt = now,
            Products = request.Products!.Select(p => new MenuProduct
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList()
        };

        await menuRepository.AddAsync(menu, cancellationToken);
        return MapToResponse(menu);
    }

    public async Task<MenuResponse?> UpdateAsync(Guid id, MenuUpsertRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var menu = await menuRepository.GetByIdForUpdateAsync(id, cancellationToken);
        if (menu is null)
        {
            return null;
        }

        var now = DateTime.UtcNow;
        menu.Name = request.Name.Trim();
        menu.Description = request.Description.Trim();
        menu.SellingPrice = request.SellingPrice;
        menu.IsActive = request.IsActive;
        menu.UpdatedAt = now;

        menu.Products.Clear();
        foreach (var product in request.Products!)
        {
            menu.Products.Add(new MenuProduct
            {
                ProductId = product.ProductId,
                Quantity = product.Quantity,
                CreatedAt = now,
                UpdatedAt = now
            });
        }

        await menuRepository.SaveChangesAsync(cancellationToken);
        return MapToResponse(menu);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var menu = await menuRepository.GetByIdForUpdateAsync(id, cancellationToken);
        if (menu is null)
        {
            return false;
        }

        await menuRepository.DeleteAsync(menu, cancellationToken);
        return true;
    }

    private static void ValidateRequest(MenuUpsertRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Description))
        {
            throw new MenuException("Name and description are required.");
        }

        if (request.SellingPrice < 0)
        {
            throw new MenuException("Menu selling price must be non-negative.");
        }

        if (request.Products is null || request.Products.Count == 0)
        {
            throw new MenuException("Menu must have at least one product.");
        }

        if (request.Products.Any(p => p.ProductId == Guid.Empty || p.Quantity <= 0))
        {
            throw new MenuException("Each menu product requires valid product_id and quantity > 0.");
        }
    }

    private static MenuResponse MapToResponse(MenuEntity menu)
    {
        return new MenuResponse(
            menu.Id,
            menu.Name,
            menu.Description,
            menu.SellingPrice,
            menu.IsActive,
            menu.CreatedAt,
            menu.UpdatedAt,
            menu.Products.Select(p => new MenuItemResponse(p.Id, p.ProductId, p.Quantity)).ToList());
    }
}
