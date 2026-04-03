using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Menu.Infrastructure.Persistence.Configurations;

public sealed class MenuEntityConfiguration : IEntityTypeConfiguration<Entities.Menu>
{
    public void Configure(EntityTypeBuilder<Entities.Menu> builder)
    {
        builder.ToTable("Menus");
        builder.HasKey(menu => menu.Id);

        builder.Property(menu => menu.BranchId).HasColumnName("branch_id");
        builder.Property(menu => menu.Name).IsRequired().HasMaxLength(200).HasColumnName("name");
        builder.Property(menu => menu.Description).IsRequired().HasColumnName("description");
        builder.Property(menu => menu.SellingPrice).HasColumnType("decimal(18,2)").HasColumnName("selling_price");
        builder.Property(menu => menu.IsActive).HasColumnName("is_active");
        builder.Property(menu => menu.CreatedAt).HasColumnName("created_at");
        builder.Property(menu => menu.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(menu => new { menu.BranchId, menu.Name });
    }
}
