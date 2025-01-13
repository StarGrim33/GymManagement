using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Application.Gyms;

public sealed record CreateGymCommand(
    Name Name, 
    Description Description, 
    Address Address, 
    Schedule Schedule) : ICommand<Guid>;