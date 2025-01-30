using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Trainers;

namespace GymManagement.Application.Gyms.GetGym;

public class GymResponse
{
    public Guid Id { get; init; }

    public Name Name { get; init; }

    public Description Description { get; init; }

    public Address Address { get; init; }

    public Schedule Schedule { get; init; }

    public List<GymAmenity> GymAmenities { get; init; } = [];

    public List<Trainer> Trainers { get; init; } = [];

    public List<GymEquipment> Equipment { get; init; } = [];

    public List<Membership> Memberships { get; init; } = [];

    public List<TrainingSession> TrainingSessions { get; init; } = [];
}