using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Trainers;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Application.Users.GetUser;

public sealed class UserResponse
{
    public Guid Id { get; init; }
    public List<Membership> Memberships { get; init; } = [];
    public Role Role { get; init; }
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public bool IsActive { get; init; }
    public List<TrainingSession> TrainingSessions { get; init; } = [];
}