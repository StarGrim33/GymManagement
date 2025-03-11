using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Trainers;
using GymManagement.Domain.Entities.Users.Events;

namespace GymManagement.Domain.Entities.Users;

public sealed class User : Entity
{
    private readonly List<Role> _roles = [];

    private User(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Email email,
        string phoneNumber,
        DateTime dateOfBirth,
        bool isActive,
        Role role, 
        Address address)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
        IsActive = isActive;
        Role = role;
        Address = address;
    }

    public User()
    {
    }

    public List<Membership> Memberships { get; private set; } = [];

    public Role Role { get; private set; }

    public Address Address { get; private set; }

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }

    public string PhoneNumber { get; private set; }

    public DateTime DateOfBirth { get; private set; }

    public bool IsActive { get; private set; }

    public string IdentityId { get; private set; } = string.Empty;

    public List<TrainingSession> TrainingSessions { get; private set; } = [];

    public string PasswordHash { get; private set; } = string.Empty;

    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public static User Create(
        FirstName firstName, 
        LastName lastName, 
        Email email, 
        string phoneNumber, 
        DateTime dateOfBirth, 
        bool isActive, 
        Role role,
        Address address)
    {
        var user = new User(
            Guid.NewGuid(),
            firstName,
            lastName,
            email,
            phoneNumber,
            dateOfBirth,
            isActive,
            role,
            address);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        user._roles.Add(role);

        return user;
    }

    internal void AddMembership(Membership membership)
    {
        ArgumentNullException.ThrowIfNull(membership);

        Memberships.Add(membership);
    }

    public Membership? GetActiveMembershipInGym(Gym gym)
    {
        return Memberships
            .FirstOrDefault(m => m.Gym == gym && m.IsActive && m.EndDate > DateTime.UtcNow);
    }

    public void SetIdentityId(string identityId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identityId);

        IdentityId = identityId;
    }

    public void SetPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        PasswordHash = HashPassword(password);
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
    }
}