using GymManagement.Domain.Entities.Gyms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Configurations;

internal sealed class GymAmenityConfiguration : IEntityTypeConfiguration<GymAmenity>
{
    public void Configure(EntityTypeBuilder<GymAmenity> builder)
    {
        builder.ToTable("gym_amenities");

        builder.HasKey(ga => new { ga.GymId, ga.Amenity });

        builder.Property(ga => ga.Amenity)
            .HasColumnName("amenity")
            .HasConversion<string>()
            .IsRequired();
    }
}
