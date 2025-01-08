using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Memberships.Events;

public record MembershipUnfrozenDomainEvent(Guid MembershipId) : IDomainEvent;