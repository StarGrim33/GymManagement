using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Memberships.Events;

public record MembershipExtendedDomainEvent(Guid MembershipId) : IDomainEvent;