using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Product.Infrastructure.Persistence.Configurations;

public sealed class ProductPriceHistoryEntityConfiguration : IEntityTypeConfiguration<Entities.ProductPriceHistory>
{
    public void Configure(EntityTypeBuilder<Entities.ProductPriceHistory> builder)
    {
        builder.ToTable("ProductPriceHistories");
        builder.HasKey(price => price.Id);

        builder.Property(price => price.ProductId).IsRequired().HasColumnName("product_id");
        builder.Property(price => price.BasePrice).HasColumnType("decimal(18,2)").HasColumnName("base_price");
        builder.Property(price => price.SellingPrice).HasColumnType("decimal(18,2)").HasColumnName("selling_price");
        builder.Property(price => price.Reason).HasMaxLength(200).HasColumnName("reason");
        builder.Property(price => price.EffectiveFrom).HasColumnName("effective_from");
        builder.Property(price => price.EffectiveTo).HasColumnName("effective_to");

        builder.HasIndex(price => new { price.ProductId, price.EffectiveFrom });
    }
}
