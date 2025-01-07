using GymManagement.Domain.Entities.Memberships.Events;
using MediatR;

namespace GymManagement.Application.Memberships.BuyMembership;

internal sealed class ExtendedMembershipDomainEventHandler
    : INotificationHandler<MembershipExtendedDomainEvent>
{
    public Task Handle(MembershipExtendedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}