using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Product.Infrastructure.Persistence.Configurations;

public sealed class ProductCategoryEntityConfiguration : IEntityTypeConfiguration<Entities.ProductCategory>
{
    public void Configure(EntityTypeBuilder<Entities.ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");
        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name).IsRequired().HasMaxLength(120).HasColumnName("name");
        builder.Property(category => category.Description).HasMaxLength(500).HasColumnName("description");
        builder.Property(category => category.DisplayOrder).HasColumnName("display_order");
        builder.Property(category => category.IsActive).HasColumnName("is_active");
        builder.Property(category => category.CreatedAt).HasColumnName("created_at");
        builder.Property(category => category.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(category => category.Name).IsUnique();
    }
}
