using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Order.Infrastructure.Persistence.Configurations;

public sealed class OrderEntityConfiguration : IEntityTypeConfiguration<Entities.Order>
{
    public void Configure(EntityTypeBuilder<Entities.Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(order => order.Id);

        builder.Property(order => order.CustomerId).HasColumnName("customer_id");
        builder.Property(order => order.UserId).IsRequired().HasColumnName("user_id");
        builder.Property(order => order.OrderType).IsRequired().HasConversion<string>().HasColumnName("order_type");
        builder.Property(order => order.Status).IsRequired().HasConversion<string>().HasColumnName("status");
        builder.Property(order => order.CancellationReason).HasMaxLength(500).HasColumnName("cancellation_reason");
        builder.Property(order => order.CreatedAt).HasColumnName("created_at");
        builder.Property(order => order.UpdatedAt).HasColumnName("updated_at");
        builder.Property(order => order.TotalPrice).HasColumnType("decimal(18,2)").HasColumnName("total_price");

        builder.HasMany(order => order.OrderDetails)
            .WithOne(orderDetails => orderDetails.Order)
            .HasForeignKey(orderDetails => orderDetails.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
