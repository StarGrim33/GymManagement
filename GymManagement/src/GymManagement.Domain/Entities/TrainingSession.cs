using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities;

public sealed class TrainingSession(Guid id) : Entity(id)
{
    public DateTime StartTime { get; private set; }

    public DateTime EndTime { get; private set; }

    public Trainer Trainer { get; private set; }

    public Gym Gym { get; private set; }

    public TrainingType TrainingType { get; private set; }

    public string Room { get; private set; }

    public bool IsCanceled { get; private set; }

    public List<User> Users { get; private set; } = [];
}