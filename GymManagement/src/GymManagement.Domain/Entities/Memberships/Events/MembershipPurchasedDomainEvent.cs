using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Memberships.Events;

public record MembershipPurchasedDomainEvent(Guid MembershipId) : IDomainEvent;