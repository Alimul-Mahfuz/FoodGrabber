using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Cart.Infrastructure.Persistence.Configurations;

public sealed class CartItemEntityConfiguration : IEntityTypeConfiguration<Entities.CartItem>
{
    public void Configure(EntityTypeBuilder<Entities.CartItem> builder)
    {
        builder.ToTable("CartItems");
        builder.HasKey(cartItem => cartItem.Id);

        builder.Property(cartItem => cartItem.CartId).HasColumnName("cart_id");
        builder.Property(cartItem => cartItem.ItemType).IsRequired().HasMaxLength(80).HasColumnName("item_type");
        builder.Property(cartItem => cartItem.Quantity).HasColumnName("quantity");
        builder.Property(cartItem => cartItem.ItemId).HasColumnName("item_id");
        builder.Property(cartItem => cartItem.CreatedAt).HasColumnName("created_at");
        builder.Property(cartItem => cartItem.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(cartItem => cartItem.CartId);
        builder.HasIndex(cartItem => new { cartItem.CartId, cartItem.ItemId, cartItem.ItemType }).IsUnique();
    }
}
