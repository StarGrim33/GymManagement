using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Gyms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Configurations;

public sealed class GymEquipmentConfiguration : IEntityTypeConfiguration<GymEquipment>
{
    public void Configure(EntityTypeBuilder<GymEquipment> builder)
    {
        builder.ToTable("gym_equipment");

        builder.HasKey(g => g.Id);

        builder.Property(u => u.Name)
            .HasConversion(
                v => v.Value,
                v => new Name(v))
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Description)
            .HasConversion(
                v => v.Value,
                v => new Description(v))
            .HasColumnName("description")
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(u => u.IsAvailable)
            .HasColumnName("is_available")
            .IsRequired();

        builder.Property(u => u.EquipmentType)
            .HasColumnName("equipment_type")
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne(u => u.Gym)
            .WithMany(gym => gym.Equipment)
            .HasForeignKey(u => u.GymId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}