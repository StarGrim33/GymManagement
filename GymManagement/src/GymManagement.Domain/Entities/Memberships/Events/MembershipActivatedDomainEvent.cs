using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Memberships.Events;

public record MembershipActivatedDomainEvent(Guid MembershipId) : IDomainEvent;