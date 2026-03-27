using FoodGrabber.Menu.Contracts;
using FoodGrabber.Order.Abstractions;
using FoodGrabber.Order.Contracts;
using FoodGrabber.Order.Entities;
using FoodGrabber.Order.Exceptions;
using FoodGrabber.Product.Contracts;
using FoodGrabber.Shared.Pagination;
using OrderEntity = FoodGrabber.Order.Entities.Order;

namespace FoodGrabber.Order.Services;

public sealed class OrderServices(
    IOrderRepository orderRepository,
    IProductReadContract productReadContract,
    IMenuReadContract menuReadContract) : IOrderService
{
    public async Task<PagedResult<OrderResponse>> GetAllAsync(PaginationQuery paginationQuery, CancellationToken ct = default)
    {
        var page = paginationQuery.NormalizedPage;
        var pageSize = paginationQuery.NormalizedPageSize;
        var orders = await orderRepository.GetPagedAsync(page, pageSize, ct);
        var totalCount = await orderRepository.CountAsync(ct);
        var items = orders.Select(MapToResponse).ToList();
        return new PagedResult<OrderResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<OrderResponse> CreateAsync(OrderUpsertRequest request, CancellationToken ct = default)
    {
        ValidateRequest(request);

        var now = DateTime.UtcNow;
        var order = new OrderEntity
        {
            CustomerId = request.CustomerId,
            UserId = request.UserId,
            OrderType = MapOrderType(request.OrderType),
            Status = OrderStatus.Unpaid,
            CreatedAt = now,
            UpdatedAt = now
        };

        foreach (var item in request.OrderDetails)
        {
            var pricedItem = await GetPriceAsync(item, ct);

            order.OrderDetails.Add(new OrderDetails
            {
                OrderId = order.Id,
                MenuId = item.MenuId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = pricedItem.UnitPrice,
                TotalPrice = pricedItem.TotalPrice
            });
        }

        order.TotalPrice = order.OrderDetails.Sum(detail => detail.TotalPrice);

        await orderRepository.AddAsync(order, ct);
        return MapToResponse(order);
    }

    private static void ValidateRequest(OrderUpsertRequest request)
    {
        if (request.OrderDetails is null || request.OrderDetails.Count == 0)
        {
            throw new OrderModuleException("Order must contain at least one product or menu.");
        }

        if (request.OrderDetails.Any(item => item.Quantity <= 0))
        {
            throw new OrderModuleException("Order item quantity must be greater than zero.");
        }

        if (request.OrderDetails.Any(item => item.MenuId.HasValue == item.ProductId.HasValue))
        {
            throw new OrderModuleException("Each order item must reference either a menu or a product.");
        }
    }

    private async Task<PricedOrderItem> GetPriceAsync(OrderDetailsRequest item, CancellationToken ct)
    {
        if (item.ProductId.HasValue)
        {
            var product = await productReadContract.GetPricingAsync(item.ProductId.Value, ct);
            if (product is null || !product.IsActive)
            {
                throw new OrderModuleException($"Product '{item.ProductId.Value}' was not found.");
            }

            return new PricedOrderItem(
                product.Id,
                item.Quantity,
                product.UnitPrice,
                product.UnitPrice * item.Quantity,
                "Product");
        }

        if (item.MenuId.HasValue)
        {
            var menu = await menuReadContract.GetPricingAsync(item.MenuId.Value, ct);
            if (menu is null || !menu.IsActive)
            {
                throw new OrderModuleException($"Menu '{item.MenuId.Value}' was not found.");
            }

            return new PricedOrderItem(
                menu.Id,
                item.Quantity,
                menu.UnitPrice,
                menu.UnitPrice * item.Quantity,
                "Menu");
        }

        throw new OrderModuleException("Order item must reference a valid product or menu.");
    }

    private static OrderType MapOrderType(OrderTypeContract orderType)
    {
        return orderType switch
        {
            OrderTypeContract.Online => OrderType.Online,
            OrderTypeContract.Offline => OrderType.Offline,
            _ => throw new OrderModuleException("Order type must be either Online or Offline.")
        };
    }

    private static OrderResponse MapToResponse(OrderEntity order)
    {
        return new OrderResponse(
            order.Id,
            order.CustomerId,
            order.UserId,
            order.OrderType.ToString(),
            order.Status.ToString(),
            order.CancellationReason,
            order.CreatedAt,
            order.UpdatedAt,
            order.TotalPrice,
            order.OrderDetails.Select(detail => new OrderDetailsResponse(
                detail.Id,
                detail.MenuId,
                detail.ProductId,
                detail.PaymentId,
                detail.Quantity,
                detail.UnitPrice,
                detail.TotalPrice)).ToList());
    }
}
