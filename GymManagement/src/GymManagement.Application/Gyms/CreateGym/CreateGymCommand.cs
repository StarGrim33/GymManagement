using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Application.Gyms.CreateGym;

public sealed record CreateGymCommand(
    Name Name,
    Description Description,
    Address Address,
    Schedule Schedule) : ICommand<Guid>;