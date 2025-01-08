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
        Address address, 
        Schedule schedule) 
        : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
    }

    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public Address Address { get; private set; }

    public Schedule Schedule { get; private set; }

    public List<Amenity> Amenities { get; private set; } = [];

    public List<Trainer> Trainers { get; private set; } = [];

    public List<GymEquipment> Equipment { get; private set; } = [];

    public List<Membership> Memberships { get; private set; } = [];

    public static Gym Create(Name name, Description description, Address address, Schedule schedule)
    {
        var gym = new Gym(
            Guid.NewGuid(),
            name,
            description,
            address,
            schedule);

        // Можно добавить генерацию доменных событий, если необходимо
        // gym.RaiseDomainEvent(new GymCreatedDomainEvent(gym.Id));

        return gym;
    }

    public void AddAmenity(Amenity amenity)
    {
        Amenities.Add(amenity);
    }

    public void RemoveAmenity(Amenity amenity)
    {
        Amenities.Remove(amenity);
    }

    public void AddTrainer(Trainer trainer)
    {
        ArgumentNullException.ThrowIfNull(trainer);

        Trainers.Add(trainer);
    }

    public void RemoveTrainer(Trainer trainer)
    {
        ArgumentNullException.ThrowIfNull(trainer);

        var foundedTrainer = Trainers.Find(x => x.Id == trainer.Id);

        if (foundedTrainer != null) Trainers.Remove(foundedTrainer);
    }

    public void AddEquipment(GymEquipment equipment)
    {
        ArgumentNullException.ThrowIfNull(equipment);

        Equipment.Add(equipment);
    }

    public void AddMembership(Membership membership)
    {
        ArgumentNullException.ThrowIfNull(membership);

        Memberships.Add(membership);
    }
}