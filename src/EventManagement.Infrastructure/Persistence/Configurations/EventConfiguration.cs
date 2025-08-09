using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(e => e.UpdatedDate).HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).IsRequired();
        builder.Property(e => e.Location).HasMaxLength(300);
        builder.Property(e => e.Status).HasConversion<string>();

        builder
            .HasMany(e => e.Registrations)
            .WithOne(r => r.Event)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
