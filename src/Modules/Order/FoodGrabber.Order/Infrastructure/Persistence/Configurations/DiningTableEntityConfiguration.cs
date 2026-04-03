using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Order.Infrastructure.Persistence.Configurations;

public sealed class DiningTableEntityConfiguration : IEntityTypeConfiguration<Entities.DiningTable>
{
    public void Configure(EntityTypeBuilder<Entities.DiningTable> builder)
    {
        builder.ToTable("DiningTables");
        builder.HasKey(table => table.Id);

        builder.Property(table => table.BranchId).IsRequired().HasColumnName("branch_id");
        builder.Property(table => table.TableCode).IsRequired().HasMaxLength(50).HasColumnName("table_code");
        builder.Property(table => table.FloorName).HasMaxLength(100).HasColumnName("floor_name");
        builder.Property(table => table.SectionName).HasMaxLength(100).HasColumnName("section_name");
        builder.Property(table => table.SeatCount).HasColumnName("seat_count");
        builder.Property(table => table.IsActive).HasColumnName("is_active");

        builder.HasIndex(table => new { table.BranchId, table.TableCode }).IsUnique();
    }
}
