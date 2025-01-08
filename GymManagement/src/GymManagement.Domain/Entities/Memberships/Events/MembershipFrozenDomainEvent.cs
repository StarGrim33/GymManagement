using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Memberships.Events;

public record MembershipFrozenDomainEvent(Guid MembershipId) : IDomainEvent;