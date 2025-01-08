using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Trainers;
using GymManagement.Domain.Entities.Users.Events;

namespace GymManagement.Domain.Entities.Users;

public sealed class User : Entity
{
    private User(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Email email,
        string phoneNumber,
        DateTime dateOfBirth,
        bool isActive,
        Roles role, Address address)
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

    public List<Membership> Memberships { get; private set; } = [];

    public Roles Role { get; private set; }

    public Address Address { get; private set; }

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }

    public string PhoneNumber { get; private set; }

    public DateTime DateOfBirth { get; private set; }

    public bool IsActive { get; private set; }

    public List<TrainingSession> TrainingSession { get; private set; } = [];

    public static User Create(
        FirstName firstName, 
        LastName lastName, 
        Email email, 
        string phoneNumber, 
        DateTime dateOfBirth, 
        bool isActive, 
        Roles role,
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

        return user;
    }

    public void AddMembership(Membership membership)
    {
        if (membership == null) throw new ArgumentNullException(nameof(membership));

        Memberships.Add(membership);
    }

    public Membership? GetActiveMembershipInGym(Gym gym)
    {
        return Memberships
            .FirstOrDefault(m => m.Gym == gym && m.IsActive && m.EndDate > DateTime.UtcNow);
    }
}