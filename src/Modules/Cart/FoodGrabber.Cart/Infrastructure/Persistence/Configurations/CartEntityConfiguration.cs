using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Cart.Infrastructure.Persistence.Configurations;

public sealed class CartEntityConfiguration : IEntityTypeConfiguration<Entities.Cart>
{
    public void Configure(EntityTypeBuilder<Entities.Cart> builder)
    {
        builder.ToTable("Carts");
        builder.HasKey(cart => cart.Id);

        builder.Property(cart => cart.UserId).HasColumnName("user_id");
        builder.Property(cart => cart.Status).IsRequired().HasConversion<string>().HasColumnName("status");
        builder.Property(cart => cart.CreatedAt).HasColumnName("created_at");
        builder.Property(cart => cart.UpdatedAt).HasColumnName("updated_at");
        builder.Property(cart => cart.TotalPrice).HasColumnType("decimal(18,2)").HasColumnName("total_price");

        builder.HasMany(cart => cart.CartItems)
            .WithOne()
            .HasForeignKey(cartItem => cartItem.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cart => cart.UserId)
            .IsUnique()
            .HasFilter("[user_id] IS NOT NULL");
    }
}
