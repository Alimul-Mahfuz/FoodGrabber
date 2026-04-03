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
        builder.Property(order => order.BranchId).HasColumnName("branch_id");
        builder.Property(order => order.DiningTableId).HasColumnName("dining_table_id");
        builder.Property(order => order.OrderType).IsRequired().HasConversion<string>().HasColumnName("order_type");
        builder.Property(order => order.ServiceType).IsRequired().HasConversion<string>().HasColumnName("service_type");
        builder.Property(order => order.Status).IsRequired().HasConversion<string>().HasColumnName("status");
        builder.Property(order => order.CancellationReason).HasMaxLength(500).HasColumnName("cancellation_reason");
        builder.Property(order => order.CreatedAt).HasColumnName("created_at");
        builder.Property(order => order.UpdatedAt).HasColumnName("updated_at");
        builder.Property(order => order.SubtotalAmount).HasColumnType("decimal(18,2)").HasColumnName("subtotal_amount");
        builder.Property(order => order.TaxAmount).HasColumnType("decimal(18,2)").HasColumnName("tax_amount");
        builder.Property(order => order.DiscountAmount).HasColumnType("decimal(18,2)").HasColumnName("discount_amount");
        builder.Property(order => order.TotalPrice).HasColumnType("decimal(18,2)").HasColumnName("total_price");

        builder.HasOne(order => order.Branch)
            .WithMany()
            .HasForeignKey(order => order.BranchId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(order => order.DiningTable)
            .WithMany()
            .HasForeignKey(order => order.DiningTableId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(order => order.OrderDetails)
            .WithOne(orderDetails => orderDetails.Order)
            .HasForeignKey(orderDetails => orderDetails.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(order => order.StatusHistory)
            .WithOne(statusHistory => statusHistory.Order)
            .HasForeignKey(statusHistory => statusHistory.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(order => order.CreatedAt);
        builder.HasIndex(order => new { order.Status, order.CreatedAt });
        builder.HasIndex(order => new { order.BranchId, order.CreatedAt });
    }
}
