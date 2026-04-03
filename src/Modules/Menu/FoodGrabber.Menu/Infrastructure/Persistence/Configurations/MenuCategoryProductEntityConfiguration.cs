using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Menu.Infrastructure.Persistence.Configurations;

public sealed class MenuCategoryProductEntityConfiguration : IEntityTypeConfiguration<Entities.MenuCategoryProduct>
{
    public void Configure(EntityTypeBuilder<Entities.MenuCategoryProduct> builder)
    {
        builder.ToTable("MenuCategoryProducts");
        builder.HasKey(categoryProduct => categoryProduct.Id);

        builder.Property(categoryProduct => categoryProduct.MenuCategoryId).IsRequired().HasColumnName("menu_category_id");
        builder.Property(categoryProduct => categoryProduct.ProductId).IsRequired().HasColumnName("product_id");
        builder.Property(categoryProduct => categoryProduct.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(categoryProduct => new { categoryProduct.MenuCategoryId, categoryProduct.ProductId }).IsUnique();
        builder.HasIndex(categoryProduct => categoryProduct.ProductId);
    }
}
