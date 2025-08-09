using EventManagement.Domain.Entities;
using EventManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(r => r.Id).IsRequired();
        builder.Property(r => r.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(r => r.UpdatedDate).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(r => r.Name).IsRequired().HasMaxLength(50);

        builder.HasData(
            new Role { Id = Guid.NewGuid(), Name = UserRoles.User.ToString() },
            new Role { Id = Guid.NewGuid(), Name = UserRoles.Admin.ToString() },
            new Role { Id = Guid.NewGuid(), Name = UserRoles.Organizer.ToString() },
            new Role { Id = Guid.NewGuid(), Name = UserRoles.Attendee.ToString() }
        );
    }
}
