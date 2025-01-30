using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Gyms.GetGym;

public sealed record GetGymQuery(Guid GymId) : IQuery<GymResponse>;