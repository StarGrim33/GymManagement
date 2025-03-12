using GymManagement.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(x => x.Id);

        builder.HasMany(role => role.Users)
            .WithMany(user => user.Roles)
            .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                r => r.HasOne<User>().WithMany().HasForeignKey("UserId"),
                r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                e => e.ToTable("user_roles")
            );

        builder.HasMany(role => role.Permissions)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "RolePermission",
                r => r.HasOne<Permission>().WithMany().HasForeignKey("PermissionId"),
                r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                e => e.ToTable("role_permissions")
            );

        builder.HasData(Role.Registered);
    }
}