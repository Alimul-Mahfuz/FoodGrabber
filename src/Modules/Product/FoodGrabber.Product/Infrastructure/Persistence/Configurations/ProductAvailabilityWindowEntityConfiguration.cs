using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Product.Infrastructure.Persistence.Configurations;

public sealed class ProductAvailabilityWindowEntityConfiguration : IEntityTypeConfiguration<Entities.ProductAvailabilityWindow>
{
    public void Configure(EntityTypeBuilder<Entities.ProductAvailabilityWindow> builder)
    {
        builder.ToTable("ProductAvailabilityWindows");
        builder.HasKey(availability => availability.Id);

        builder.Property(availability => availability.ProductId).IsRequired().HasColumnName("product_id");
        builder.Property(availability => availability.DayOfWeek).HasConversion<int>().HasColumnName("day_of_week");
        builder.Property(availability => availability.StartTime).HasColumnName("start_time");
        builder.Property(availability => availability.EndTime).HasColumnName("end_time");
        builder.Property(availability => availability.IsAvailable).HasColumnName("is_available");

        builder.HasIndex(availability => new
        {
            availability.ProductId,
            availability.DayOfWeek,
            availability.StartTime,
            availability.EndTime
        }).IsUnique();
    }
}
