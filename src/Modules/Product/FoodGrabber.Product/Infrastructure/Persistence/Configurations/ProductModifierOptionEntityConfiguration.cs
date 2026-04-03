using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Product.Infrastructure.Persistence.Configurations;

public sealed class ProductModifierOptionEntityConfiguration : IEntityTypeConfiguration<Entities.ProductModifierOption>
{
    public void Configure(EntityTypeBuilder<Entities.ProductModifierOption> builder)
    {
        builder.ToTable("ProductModifierOptions");
        builder.HasKey(option => option.Id);

        builder.Property(option => option.ProductModifierGroupId).IsRequired().HasColumnName("product_modifier_group_id");
        builder.Property(option => option.Name).IsRequired().HasMaxLength(120).HasColumnName("name");
        builder.Property(option => option.PriceDelta).HasColumnType("decimal(18,2)").HasColumnName("price_delta");
        builder.Property(option => option.IsDefault).HasColumnName("is_default");
        builder.Property(option => option.DisplayOrder).HasColumnName("display_order");
        builder.Property(option => option.IsActive).HasColumnName("is_active");

        builder.HasIndex(option => new { option.ProductModifierGroupId, option.Name }).IsUnique();
    }
}
