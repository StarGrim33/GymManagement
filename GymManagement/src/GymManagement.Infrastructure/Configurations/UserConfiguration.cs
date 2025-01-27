using GymManagement.Domain.Entities.Trainers;
using GymManagement.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.DateOfBirth)
            .HasColumnName("date_of_birth")
            .IsRequired();

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasConversion(
                v => v.Value,
                v => new FirstName(v))
            .HasColumnName("first_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(l => l.LastName)
            .HasConversion(
                v => v.Value,
                v => new LastName(v))
            .HasColumnName("last_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(em => em.Email)
            .HasConversion(
                v => v.Value,
                v => new Domain.Entities.Email(v))
            .HasColumnName("email")
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(u => u.Address, a =>
        {
            a.Property(addr => addr.Street)
                .HasColumnName("street")
                .HasMaxLength(100)
                .IsRequired();

            a.Property(addr => addr.City)
                .HasColumnName("city")
                .HasMaxLength(50)
                .IsRequired();

            a.Property(addr => addr.ZipCode)
                .HasColumnName("zip_code")
                .HasMaxLength(20)
                .IsRequired();
        });

        builder.HasMany(u => u.Memberships)
            .WithOne(m => m.User)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.TrainingSessions)
            .WithMany(ts => ts.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserTrainingSession",
                j => j
                    .HasOne<TrainingSession>()
                    .WithMany()
                    .HasForeignKey("TrainingSessionId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
            );

        builder.HasIndex(u => u.Email).IsUnique();
    }
}