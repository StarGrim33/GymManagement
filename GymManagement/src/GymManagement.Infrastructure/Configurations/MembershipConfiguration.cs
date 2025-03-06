using GymManagement.Domain.Entities.Memberships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Configurations;

internal sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("memberships");

        builder.HasKey(membership => membership.Id);

        builder.Property(membership => membership.PriceAmount)
            .HasColumnName("price_amount")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(membership => membership.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(membership => membership.StartDate)
            .HasColumnName("start_date")
            .IsRequired(false);

        builder.Property(membership => membership.EndDate)
            .HasColumnName("end_date")
            .IsRequired(false);

        builder.Property(membership => membership.MembershipStatus)
            .HasColumnName("membership_status")
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne(membership => membership.User)
            .WithMany(user => user.Memberships)
            .HasForeignKey(membership => membership.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(membership => membership.Gym)
            .WithMany(gym => gym.Memberships) 
            .HasForeignKey(membership => membership.GymId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(membership => membership.MembershipType)
            .WithMany(membership => membership.Memberships)
            .HasForeignKey(membership => membership.MembershipTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasMany(m => m.Invoices)
            .WithOne(i => i.Membership)
            .HasForeignKey(i => i.MembershipId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<uint>("Version").IsRowVersion();
    }
}