using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Product.Infrastructure.Persistence.Configurations;

public sealed class ProductStockEntryEntityConfiguration : IEntityTypeConfiguration<Entities.ProductStockEntry>
{
    public void Configure(EntityTypeBuilder<Entities.ProductStockEntry> builder)
    {
        builder.ToTable("ProductStockEntries");
        builder.HasKey(stockEntry => stockEntry.Id);

        builder.Property(stockEntry => stockEntry.ProductId).HasColumnName("product_id");
        builder.Property(stockEntry => stockEntry.Quantity).HasColumnType("decimal(18,3)").HasColumnName("quantity");
        builder.Property(stockEntry => stockEntry.MovementType).IsRequired().HasMaxLength(30).HasColumnName("movement_type");
        builder.Property(stockEntry => stockEntry.Unit).IsRequired().HasMaxLength(30).HasColumnName("unit");
        builder.Property(stockEntry => stockEntry.Notes).HasMaxLength(500).HasColumnName("notes");
        builder.Property(stockEntry => stockEntry.UserId).IsRequired().HasColumnName("user_id");
        builder.Property(stockEntry => stockEntry.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(stockEntry => new { stockEntry.ProductId, stockEntry.CreatedAt })
            .HasDatabaseName("IX_ProductStockEntries_product_id_created_at");
    }
}
