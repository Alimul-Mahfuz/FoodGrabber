using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Order.Infrastructure.Persistence.Configurations;

public sealed class OrderItemModifierEntityConfiguration : IEntityTypeConfiguration<Entities.OrderItemModifier>
{
    public void Configure(EntityTypeBuilder<Entities.OrderItemModifier> builder)
    {
        builder.ToTable("OrderItemModifiers");
        builder.HasKey(modifier => modifier.Id);

        builder.Property(modifier => modifier.OrderDetailsId).IsRequired().HasColumnName("order_details_id");
        builder.Property(modifier => modifier.ModifierGroupName).IsRequired().HasMaxLength(120).HasColumnName("modifier_group_name");
        builder.Property(modifier => modifier.ModifierOptionName).IsRequired().HasMaxLength(120).HasColumnName("modifier_option_name");
        builder.Property(modifier => modifier.Quantity).HasColumnName("quantity");
        builder.Property(modifier => modifier.UnitPriceDelta).HasColumnType("decimal(18,2)").HasColumnName("unit_price_delta");
        builder.Property(modifier => modifier.TotalPriceDelta).HasColumnType("decimal(18,2)").HasColumnName("total_price_delta");

        builder.HasIndex(modifier => modifier.OrderDetailsId);
    }
}
