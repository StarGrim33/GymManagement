using GymManagement.Domain.Entities.Gyms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Configurations;

internal sealed class GymConfiguration : IEntityTypeConfiguration<Gym>
{
    public void Configure(EntityTypeBuilder<Gym> builder)
    {
        builder.ToTable("gym");

        builder.HasKey(g => g.Id);

        builder.OwnsOne(g => g.Name, n =>
        {
            n.Property(name => name.Value)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(g => g.Description, d =>
        {
            d.Property(desc => desc.Value)
                .HasColumnName("description")
                .HasColumnType("text")
                .IsRequired();
        });

        builder.OwnsOne(g => g.Address, a =>
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

        builder.OwnsOne(g => g.Schedule, s =>
        {
            s.Property(schedule => schedule.Value)
                .HasColumnName("schedule")
                .HasColumnType("text")
                .IsRequired();
        });

        builder.HasMany(g => g.Trainers)
            .WithOne(t => t.Gym)
            .HasForeignKey(t => t.GymId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(g => g.Equipment)
            .WithOne(e => e.Gym)
            .HasForeignKey(e => e.GymId)
            .OnDelete(DeleteBehavior.Restrict);

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
    }
}