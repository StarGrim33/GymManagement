using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities;

public sealed class Gym(Guid id) : Entity(id)
{
    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public Address Address { get; private set; }

    public string PhoneNumber { get; private set; } = string.Empty;

    public string Schedule { get; private set; } = string.Empty;

    public List<Amenity> Amenities { get; private set; } = [];

    public List<Trainer> Trainers { get; private set; } = [];

    public List<GymEquipment> Equipment { get; private set; } = [];

    public List<Membership> Memberships { get; private set; } = [];
}