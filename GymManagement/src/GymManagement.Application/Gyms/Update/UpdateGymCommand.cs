using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities;
using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Gyms.Update;

public record UpdateGymCommand(Guid GymId,
    Name Name,
    Description Description,
    Address Address,
    Schedule Schedule) : ICommand<Guid>;