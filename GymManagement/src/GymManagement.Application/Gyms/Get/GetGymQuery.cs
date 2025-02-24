using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Gyms.Get;

public sealed record GetGymQuery(Guid GymId) : IQuery<GymResponse>;