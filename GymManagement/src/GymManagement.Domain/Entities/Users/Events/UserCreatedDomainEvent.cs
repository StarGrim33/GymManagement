using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;