using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Trainers;

namespace GymManagement.Application.Gyms.Get;

public class GymResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string Schedule { get; init; } = string.Empty;
    public List<GymAmenity> GymAmenities { get; init; } = [];
    public List<Trainer> Trainers { get; init; } = [];
    public List<GymEquipment> Equipment { get; init; } = [];
    public List<Membership> Memberships { get; init; } = [];
    public List<TrainingSession> TrainingSessions { get; init; } = [];
}