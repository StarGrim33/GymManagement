using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Domain.Entities.Trainers;

public sealed class Trainer : Entity
{
    private Trainer(Guid id, FirstName firstName, LastName lastName, Email email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    private Trainer()
    {
    }

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }

    public string PhoneNumber { get; private set; } = string.Empty;

    public string Specialization { get; private set; } = string.Empty;

    public string Bio { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public Gym Gym { get; private set; }

    public Guid GymId { get; private set; }

    public List<TrainingSession> TrainingSessions { get; private set; } = [];
}