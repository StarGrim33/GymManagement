using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Gyms.Create;

public sealed record CreateGymCommand(
    string Name,
    string Description,
    string Street,
    string City,
    string ZipCode,
    string Schedule) : ICommand<Guid>;