using GymManagement.Domain.Entities.Gyms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Configurations;

internal sealed class GymConfiguration : IEntityTypeConfiguration<Gym>
{
    public void Configure(EntityTypeBuilder<Gym> builder)
    {
        builder.ToTable("gyms");

        builder.HasKey(g => g.Id);

        // Простые свойства без Value Objects
        builder.Property(g => g.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Description)
            .HasColumnName("description")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(g => g.Street)
            .HasColumnName("street")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.City)
            .HasColumnName("city")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(g => g.ZipCode)
            .HasColumnName("zip_code")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(g => g.Schedule)
            .HasColumnName("schedule")
            .HasColumnType("text")
            .IsRequired();

        // Настройка backing fields для приватных коллекций
        builder.Metadata
            .FindNavigation(nameof(Gym.GymAmenities))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata
            .FindNavigation(nameof(Gym.Trainers))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata
            .FindNavigation(nameof(Gym.Equipment))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata
            .FindNavigation(nameof(Gym.Memberships))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata
            .FindNavigation(nameof(Gym.TrainingSessions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        // Отношения
        builder.HasMany(g => g.Trainers)
            .WithOne(t => t.Gym)
            .HasForeignKey(t => t.GymId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(g => g.Equipment)
            .WithOne(e => e.Gym)
            .HasForeignKey(e => e.GymId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Memberships)
            .WithOne(m => m.Gym)
            .HasForeignKey(m => m.GymId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(g => g.TrainingSessions)
            .WithOne(ts => ts.Gym)
            .HasForeignKey(ts => ts.GymId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(g => g.GymAmenities)
            .WithOne(ga => ga.Gym)
            .HasForeignKey(ga => ga.GymId)
            .OnDelete(DeleteBehavior.Cascade);

        // Индексы
        builder.HasIndex(g => g.Name)
            .IsUnique()
            .HasDatabaseName("IX_Gyms_Name");
    }
}