using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Memberships.Events;

public record MembershipPaidDomainEvent(Guid MembershipId) : IDomainEvent;