using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Gyms.Delete;

public record DeleteGymCommand(Guid GymId) : ICommand<Guid>;