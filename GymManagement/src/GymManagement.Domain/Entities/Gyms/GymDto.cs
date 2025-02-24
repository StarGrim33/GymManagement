using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Trainers;

namespace GymManagement.Domain.Entities.Gyms;

public class GymDto
{
    public Guid Id { get; init; }

    public string? Name { get; init; }

    public string? Description { get; init; }

    public AddressDto? Address { get; init; }

    public string? Schedule { get; init; }

    public List<GymAmenity>? GymAmenities { get; init; }

    public List<Trainer>? Trainers { get; init; }

    public List<GymEquipment>? Equipment { get; init; }

    public List<Membership>? Memberships { get; init; }

    public List<TrainingSession>? TrainingSessions { get; init; }
}