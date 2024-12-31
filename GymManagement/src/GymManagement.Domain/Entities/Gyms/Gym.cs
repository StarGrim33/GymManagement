using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Invoices;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Trainers;

namespace GymManagement.Domain.Entities.Gyms;

public sealed class Gym : Entity
{
    private Gym(
        Guid id, 
        Name name, 
        Description description, 
        Address address) 
        : base(id)
    {
        Name = name;
        Description = description;
        Address = address;
    }

    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public Address Address { get; private set; }

    public string Schedule { get; private set; } = string.Empty;

    public List<Amenity> Amenities { get; private set; } = [];

    public List<Trainer> Trainers { get; private set; } = [];

    public List<GymEquipment> Equipment { get; private set; } = [];

    public List<Membership> Memberships { get; private set; } = [];

    public void AddMembership(Membership membership)
    {
        if (membership == null) throw new ArgumentNullException(nameof(membership));

        Memberships.Add(membership);
    }
}