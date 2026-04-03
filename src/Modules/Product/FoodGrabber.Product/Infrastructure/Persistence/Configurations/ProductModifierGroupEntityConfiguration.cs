using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Product.Infrastructure.Persistence.Configurations;

public sealed class ProductModifierGroupEntityConfiguration : IEntityTypeConfiguration<Entities.ProductModifierGroup>
{
    public void Configure(EntityTypeBuilder<Entities.ProductModifierGroup> builder)
    {
        builder.ToTable("ProductModifierGroups");
        builder.HasKey(group => group.Id);

        builder.Property(group => group.ProductId).IsRequired().HasColumnName("product_id");
        builder.Property(group => group.Name).IsRequired().HasMaxLength(120).HasColumnName("name");
        builder.Property(group => group.MinSelections).HasColumnName("min_selections");
        builder.Property(group => group.MaxSelections).HasColumnName("max_selections");
        builder.Property(group => group.IsRequired).HasColumnName("is_required");
        builder.Property(group => group.DisplayOrder).HasColumnName("display_order");
        builder.Property(group => group.IsActive).HasColumnName("is_active");

        builder.HasMany(group => group.Options)
            .WithOne(option => option.ProductModifierGroup)
            .HasForeignKey(option => option.ProductModifierGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(group => new { group.ProductId, group.Name }).IsUnique();
    }
}
