using GymManagement.Domain.Entities.Trainers;
using GymManagement.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Configurations;

internal sealed class TrainingSessionConfiguration : IEntityTypeConfiguration<TrainingSession>
{
    public void Configure(EntityTypeBuilder<TrainingSession> builder)
    {
        builder.ToTable("training_sessions");

        builder.HasKey(t => t.Id);

        builder.Property(ts => ts.StartTime)
            .HasColumnName("start_time")
            .IsRequired();

        builder.Property(ts => ts.EndTime)
            .HasColumnName("end_time")
            .IsRequired();

        builder.Property(ts => ts.Room)
            .HasColumnName("room")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ts => ts.IsCanceled)
            .HasColumnName("is_canceled")
            .IsRequired();

        builder.Property(ts => ts.TrainingType)
            .HasColumnName("training_type")
            .HasConversion<string>()
            .IsRequired();
        
        builder.HasOne(ts => ts.Trainer)
            .WithMany(tr => tr.TrainingSessions)
            .HasForeignKey(ts => ts.TrainerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(ts => ts.Gym)
            .WithMany(g => g.TrainingSessions)
            .HasForeignKey(ts => ts.GymId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasMany(ts => ts.Users)
            .WithMany(u => u.TrainingSessions)
            .UsingEntity<Dictionary<string, object>>(
                "UserTrainingSession",
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<TrainingSession>()
                    .WithMany()
                    .HasForeignKey("TrainingSessionId")
                    .OnDelete(DeleteBehavior.Cascade)
            );
    }
}