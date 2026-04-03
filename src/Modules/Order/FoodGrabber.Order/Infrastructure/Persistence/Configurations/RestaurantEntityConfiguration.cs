using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Order.Infrastructure.Persistence.Configurations;

public sealed class RestaurantEntityConfiguration : IEntityTypeConfiguration<Entities.Restaurant>
{
    public void Configure(EntityTypeBuilder<Entities.Restaurant> builder)
    {
        builder.ToTable("Restaurants");
        builder.HasKey(restaurant => restaurant.Id);

        builder.Property(restaurant => restaurant.Name).IsRequired().HasMaxLength(200).HasColumnName("name");
        builder.Property(restaurant => restaurant.LegalName).HasMaxLength(200).HasColumnName("legal_name");
        builder.Property(restaurant => restaurant.TaxRegistrationNumber).HasMaxLength(100).HasColumnName("tax_registration_number");
        builder.Property(restaurant => restaurant.Phone).HasMaxLength(30).HasColumnName("phone");
        builder.Property(restaurant => restaurant.Email).HasMaxLength(200).HasColumnName("email");
        builder.Property(restaurant => restaurant.IsActive).HasColumnName("is_active");
        builder.Property(restaurant => restaurant.CreatedAt).HasColumnName("created_at");
        builder.Property(restaurant => restaurant.UpdatedAt).HasColumnName("updated_at");

        builder.HasMany(restaurant => restaurant.Branches)
            .WithOne(branch => branch.Restaurant)
            .HasForeignKey(branch => branch.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(restaurant => restaurant.Name).IsUnique();
    }
}
