using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodGrabber.Order.Infrastructure.Persistence.Configurations;

public sealed class OrderStatusHistoryEntityConfiguration : IEntityTypeConfiguration<Entities.OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<Entities.OrderStatusHistory> builder)
    {
        builder.ToTable("OrderStatusHistory");
        builder.HasKey(history => history.Id);

        builder.Property(history => history.OrderId).IsRequired().HasColumnName("order_id");
        builder.Property(history => history.PreviousStatus).HasConversion<string>().HasColumnName("previous_status");
        builder.Property(history => history.CurrentStatus).IsRequired().HasConversion<string>().HasColumnName("current_status");
        builder.Property(history => history.ChangedBy).IsRequired().HasMaxLength(100).HasColumnName("changed_by");
        builder.Property(history => history.Note).HasMaxLength(500).HasColumnName("note");
        builder.Property(history => history.ChangedAt).HasColumnName("changed_at");

        builder.HasIndex(history => new { history.OrderId, history.ChangedAt });
    }
}
