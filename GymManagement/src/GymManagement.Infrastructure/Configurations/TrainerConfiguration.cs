using GymManagement.Domain.Entities.Trainers;
using GymManagement.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Configurations;

public sealed class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.ToTable("trainers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.FirstName)
            .HasConversion(firstName => firstName.Value, value => new FirstName(value))
            .HasColumnName("first_name");

        builder.Property(t => t.LastName)
            .HasConversion(lastName => lastName.Value, value => new LastName(value))
            .HasColumnName("last_name");

        builder.Property(t => t.Email)
            .HasConversion(email => email.Value, value => new Domain.Entities.Email(value))
            .HasColumnName("email");

        builder.Property(t => t.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(t => t.Specialization)
            .HasColumnName("specialization")
            .IsRequired(false);

        builder.Property(t => t.Bio)
            .HasColumnName("bio")
            .IsRequired(false);

        builder.HasOne(t => t.Gym)
            .WithMany(g => g.Trainers)
            .HasForeignKey(t => t.GymId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.TrainingSessions)
            .WithOne(ts => ts.Trainer)
            .HasForeignKey(ts => ts.TrainerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}