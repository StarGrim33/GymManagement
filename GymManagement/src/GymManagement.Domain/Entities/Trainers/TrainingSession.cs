using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Domain.Entities.Trainers;

public sealed class TrainingSession : Entity
{
    private TrainingSession(Guid id) 
        : base(id)
    {
    }

    private TrainingSession()
    {
    }

    public DateTime StartTime { get; private set; }

    public DateTime EndTime { get; private set; }

    public Trainer Trainer { get; private set; }

    public Guid TrainerId { get; private set; }

    public Gym Gym { get; private set; }

    public Guid GymId { get; private set; }

    public TrainingType TrainingType { get; private set; }

    public string Room { get; private set; }

    public bool IsCanceled { get; private set; }

    public List<User> Users { get; private set; } = [];
}