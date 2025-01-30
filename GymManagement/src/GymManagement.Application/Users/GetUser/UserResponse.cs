using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Trainers;
using GymManagement.Domain.Entities.Users;
using GymManagement.Domain.Entities;

namespace GymManagement.Application.Users.GetUser;

public sealed class UserResponse
{

    public Guid Id { get; init; }

    public List<Membership> Memberships { get; init; } = [];

    public Roles Role { get; init; }

    public Address? Address { get; init; }

    public FirstName? FirstName { get; init; }

    public LastName? LastName { get; init; }

    public Email? Email { get; init; }

    public string? PhoneNumber { get; init; }

    public DateTime DateOfBirth { get; init; }

    public bool IsActive { get; init; }

    public List<TrainingSession> TrainingSessions { get; init; } = [];
}