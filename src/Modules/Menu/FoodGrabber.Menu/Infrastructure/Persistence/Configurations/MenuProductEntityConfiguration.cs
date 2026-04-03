using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Menu.Infrastructure.Persistence.Configurations;

public sealed class MenuProductEntityConfiguration : IEntityTypeConfiguration<Entities.MenuProduct>
{
    public void Configure(EntityTypeBuilder<Entities.MenuProduct> builder)
    {
        builder.ToTable("MenuProducts");
        builder.HasKey(menuProduct => menuProduct.Id);

        builder.Property(menuProduct => menuProduct.MenuId).IsRequired().HasColumnName("menu_id");
        builder.Property(menuProduct => menuProduct.ProductId).IsRequired().HasColumnName("product_id");
        builder.Property(menuProduct => menuProduct.Quantity).IsRequired().HasColumnName("quantity");
        builder.Property(menuProduct => menuProduct.CreatedAt).HasColumnName("created_at");
        builder.Property(menuProduct => menuProduct.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne(menuProduct => menuProduct.Menu)
            .WithMany(menu => menu.Products)
            .HasForeignKey(menuProduct => menuProduct.MenuId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
