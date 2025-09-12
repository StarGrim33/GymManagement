using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Gyms.Update;

public record UpdateGymCommand(
    Guid GymId,
    string Name,
    string Description,
    string Street,
    string City,
    string ZipCode,
    string Region,
    string Country) : ICommand<Guid>;