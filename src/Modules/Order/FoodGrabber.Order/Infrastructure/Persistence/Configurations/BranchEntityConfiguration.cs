using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Order.Infrastructure.Persistence.Configurations;

public sealed class BranchEntityConfiguration : IEntityTypeConfiguration<Entities.Branch>
{
    public void Configure(EntityTypeBuilder<Entities.Branch> builder)
    {
        builder.ToTable("Branches");
        builder.HasKey(branch => branch.Id);

        builder.Property(branch => branch.RestaurantId).IsRequired().HasColumnName("restaurant_id");
        builder.Property(branch => branch.Name).IsRequired().HasMaxLength(200).HasColumnName("name");
        builder.Property(branch => branch.AddressLine1).HasMaxLength(200).HasColumnName("address_line_1");
        builder.Property(branch => branch.AddressLine2).HasMaxLength(200).HasColumnName("address_line_2");
        builder.Property(branch => branch.City).HasMaxLength(100).HasColumnName("city");
        builder.Property(branch => branch.State).HasMaxLength(100).HasColumnName("state");
        builder.Property(branch => branch.PostalCode).HasMaxLength(20).HasColumnName("postal_code");
        builder.Property(branch => branch.CountryCode).HasMaxLength(5).HasColumnName("country_code");
        builder.Property(branch => branch.TimeZone).IsRequired().HasMaxLength(80).HasColumnName("time_zone");
        builder.Property(branch => branch.Phone).HasMaxLength(30).HasColumnName("phone");
        builder.Property(branch => branch.IsActive).HasColumnName("is_active");
        builder.Property(branch => branch.CreatedAt).HasColumnName("created_at");
        builder.Property(branch => branch.UpdatedAt).HasColumnName("updated_at");

        builder.HasMany(branch => branch.Tables)
            .WithOne(table => table.Branch)
            .HasForeignKey(table => table.BranchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(branch => new { branch.RestaurantId, branch.Name }).IsUnique();
    }
}
