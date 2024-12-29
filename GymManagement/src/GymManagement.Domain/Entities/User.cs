using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities;

public sealed class User(Guid id) : Entity(id)
{
    public List<Membership> Memberships { get; private set; } = [];

    public Roles Role { get; private set; }

    public Address Address { get; private set; }

    public Name Name { get; private set; }

    public string PhoneNumber { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public DateTime DateOfBirth { get; private set; }

    public bool IsActive { get; private set; }

    public List<TrainingSession> TrainingSession { get; private set; } = [];
}