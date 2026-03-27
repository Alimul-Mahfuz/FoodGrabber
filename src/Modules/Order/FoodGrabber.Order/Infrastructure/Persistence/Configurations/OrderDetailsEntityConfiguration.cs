using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Order.Infrastructure.Persistence.Configurations;

public sealed class OrderDetailsEntityConfiguration : IEntityTypeConfiguration<Entities.OrderDetails>
{
    public void Configure(EntityTypeBuilder<Entities.OrderDetails> builder)
    {
        builder.ToTable("OrderDetails");
        builder.HasKey(orderDetails => orderDetails.Id);

        builder.Property(orderDetails => orderDetails.OrderId).IsRequired().HasColumnName("order_id");
        builder.Property(orderDetails => orderDetails.PaymentId).IsRequired().HasColumnName("payment_id");
        builder.Property(orderDetails => orderDetails.MenuId).HasColumnName("menu_id");
        builder.Property(orderDetails => orderDetails.ProductId).HasColumnName("product_id");
        builder.Property(orderDetails => orderDetails.Quantity).IsRequired().HasColumnName("quantity");
        builder.Property(orderDetails => orderDetails.UnitPrice).HasColumnType("decimal(18,2)").HasColumnName("unit_price");
        builder.Property(orderDetails => orderDetails.TotalPrice).HasColumnType("decimal(18,2)").HasColumnName("total_price");
    }
}
