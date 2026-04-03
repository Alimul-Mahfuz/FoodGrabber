using FoodGrabber.Identity.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Infrastructure.Data.Configurations;

public sealed class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(customer => customer.Id);

        builder.Property(customer => customer.UserId).IsRequired().HasColumnName("user_id");
        builder.Property(customer => customer.FullName).HasMaxLength(200).HasColumnName("full_name");
        builder.Property(customer => customer.Address1).HasMaxLength(300).HasColumnName("address_1");
        builder.Property(customer => customer.Address2).HasMaxLength(300).HasColumnName("address_2");
        builder.Property(customer => customer.Phone1).HasMaxLength(30).HasColumnName("phone_1");
        builder.Property(customer => customer.Phone2).HasMaxLength(30).HasColumnName("phone_2");
        builder.Property(customer => customer.Email).HasMaxLength(256).HasColumnName("email");
        builder.Property(customer => customer.Image).HasMaxLength(500).HasColumnName("image");
        builder.Property(customer => customer.IsActive).HasColumnName("is_active");

        builder.HasIndex(customer => customer.UserId).IsUnique();

        builder.HasOne(customer => customer.User)
            .WithOne(user => user.Customer)
            .HasForeignKey<Customer>(customer => customer.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
