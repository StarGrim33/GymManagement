using System.Text.Json;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.OperatingHoursModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Configurations;

internal sealed class GymConfiguration : IEntityTypeConfiguration<Gym>
{
    public void Configure(EntityTypeBuilder<Gym> builder)
    {
        builder.ToTable("gyms");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Description)
            .HasColumnName("description")
            .HasColumnType("text")
            .IsRequired();

        // Owned Address -> keep same column names as before for simpler migration
        builder.OwnsOne(g => g.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Street)
                .HasColumnName("street")
                .HasMaxLength(100)
                .IsRequired();
            addressBuilder.Property(a => a.City)
                .HasColumnName("city")
                .HasMaxLength(50)
                .IsRequired();
            addressBuilder.Property(a => a.ZipCode)
                .HasColumnName("zip_code")
                .HasMaxLength(20)
                .IsRequired();
            addressBuilder.Property(a => a.Region)
                .HasColumnName("region")
                .HasMaxLength(50);
            addressBuilder.Property(a => a.Country)
                .HasColumnName("country")
                .HasMaxLength(50);
        });

        // OperatingHours stored as JSON (PostgreSQL jsonb) – serialize/deserialize
        builder.Property(g => g.OperatingHours)
            .HasColumnName("operating_hours")
            .HasColumnType("jsonb")
            .IsRequired();

        // Backing field access for collections
        builder.Metadata.FindNavigation(nameof(Gym.GymAmenities))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(Gym.Trainers))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(Gym.Equipment))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(Gym.Memberships))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(Gym.TrainingSessions))!.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Relationships
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

        builder.HasIndex(g => g.Name)
            .IsUnique()
            .HasDatabaseName("IX_Gyms_Name");
    }
}