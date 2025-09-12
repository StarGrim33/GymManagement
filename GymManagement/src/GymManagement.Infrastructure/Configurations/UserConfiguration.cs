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

        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(100)
            .IsRequired();

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

        builder.Property(u => u.IdentityId)
            .HasColumnName("identity_id")
            .HasMaxLength(50);

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(500);

        // Адрес как отдельные поля
        builder.Property(u => u.Street)
            .HasColumnName("street")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.City)
            .HasColumnName("city")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.ZipCode)
            .HasColumnName("zip_code")
            .HasMaxLength(20)
            .IsRequired();

        // Отношения
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

        // Индексы
        builder.HasIndex(u => u.IdentityId).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasOne(u => u.Role)
            .WithMany();
    }
}