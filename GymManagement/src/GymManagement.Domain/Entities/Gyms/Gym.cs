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

    public Gym()
    {
    }

    public Name Name { get;  set; }

    public Description Description { get;  set; }

    public Address Address { get;  set; }

    public Schedule Schedule { get;  set; }

    public List<GymAmenity> GymAmenities { get; private set; } = [];

    public List<Trainer> Trainers { get; private set; } = [];

    public List<GymEquipment> Equipment { get; private set; } = [];

    public List<Membership> Memberships { get; private set; } = [];

    public List<TrainingSession> TrainingSessions { get; private set; } = [];

    public IReadOnlyCollection<Amenity> Amenities => GymAmenities.Select(ga => ga.Amenity).ToList();

    public static Gym Create(
        Name name, 
        Description description, 
        Address address, 
        Schedule schedule)
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

    public void Update(
        Name name,
        Description description,
        Address address,
        Schedule schedule)
    {
        Name = name;
        Description = description;
        Address = address;
        Schedule = schedule;
    }

    public void AddAmenity(GymAmenity amenity)
    {
        ArgumentNullException.ThrowIfNull(amenity);

        GymAmenities.Add(amenity);
    }

    public void RemoveAmenity(GymAmenity amenity)
    {
        ArgumentNullException.ThrowIfNull(amenity);

        GymAmenities.Remove(amenity);
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