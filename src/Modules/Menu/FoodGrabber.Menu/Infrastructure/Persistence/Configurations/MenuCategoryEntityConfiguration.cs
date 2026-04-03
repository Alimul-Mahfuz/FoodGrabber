using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Menu.Infrastructure.Persistence.Configurations;

public sealed class MenuCategoryEntityConfiguration : IEntityTypeConfiguration<Entities.MenuCategory>
{
    public void Configure(EntityTypeBuilder<Entities.MenuCategory> builder)
    {
        builder.ToTable("MenuCategories");
        builder.HasKey(category => category.Id);

        builder.Property(category => category.BranchId).HasColumnName("branch_id");
        builder.Property(category => category.Name).IsRequired().HasMaxLength(120).HasColumnName("name");
        builder.Property(category => category.Description).HasMaxLength(500).HasColumnName("description");
        builder.Property(category => category.DisplayOrder).HasColumnName("display_order");
        builder.Property(category => category.IsActive).HasColumnName("is_active");
        builder.Property(category => category.CreatedAt).HasColumnName("created_at");
        builder.Property(category => category.UpdatedAt).HasColumnName("updated_at");

        builder.HasMany(category => category.Products)
            .WithOne(menuCategoryProduct => menuCategoryProduct.MenuCategory)
            .HasForeignKey(menuCategoryProduct => menuCategoryProduct.MenuCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(category => new { category.BranchId, category.Name }).IsUnique();
    }
}
