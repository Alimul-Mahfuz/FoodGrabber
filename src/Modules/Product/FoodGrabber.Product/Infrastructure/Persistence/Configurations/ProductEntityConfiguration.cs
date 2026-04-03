using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Product.Infrastructure.Persistence.Configurations;

public sealed class ProductEntityConfiguration : IEntityTypeConfiguration<Entities.Product>
{
    public void Configure(EntityTypeBuilder<Entities.Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name).IsRequired().HasMaxLength(200).HasColumnName("name");
        builder.Property(product => product.Description).IsRequired().HasColumnName("description");
        builder.Property(product => product.CurrentStock).HasColumnType("decimal(18,3)").HasColumnName("current_stock");
        builder.Property(product => product.StockUnit).IsRequired().HasMaxLength(30).HasColumnName("stock_unit");
        builder.Property(product => product.BasePrice).HasColumnType("decimal(18,2)").HasColumnName("base_price");
        builder.Property(product => product.SellingPrice).HasColumnType("decimal(18,2)").HasColumnName("selling_price");
        builder.Property(product => product.Image).HasMaxLength(500).HasColumnName("image");
        builder.Property(product => product.Tags).HasMaxLength(500).HasColumnName("tags");
        builder.Property(product => product.IsActive).HasColumnName("is_active");
        builder.Property(product => product.UserId).IsRequired().HasColumnName("user_id");
        builder.Property(product => product.CreatedAt).HasColumnName("created_at");
        builder.Property(product => product.UpdatedAt).HasColumnName("updated_at");

        builder.HasMany(product => product.StockEntries)
            .WithOne(stockEntry => stockEntry.Product)
            .HasForeignKey(stockEntry => stockEntry.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
