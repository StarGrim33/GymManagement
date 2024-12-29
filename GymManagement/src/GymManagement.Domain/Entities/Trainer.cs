using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities;

public sealed class Trainer(Guid id) : Entity(id)
{
    public Name Name { get; private set; }

    public string PhoneNumber { get; private set; } = string.Empty;

    public string Specialization { get; private set; } = string.Empty;

    public string Bio { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public List<Gym> Gym { get; private set; } = [];

    public List<TrainingSession> TrainingSessions { get; private set; } = [];
}