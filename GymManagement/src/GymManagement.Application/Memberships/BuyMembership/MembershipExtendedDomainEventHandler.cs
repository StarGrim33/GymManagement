using GymManagement.Domain.Entities.Memberships.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GymManagement.Application.Memberships.BuyMembership;

internal sealed class MembershipExtendedDomainEventHandler(ILogger<MembershipExtendedDomainEventHandler> logger) : INotificationHandler<MembershipExtendedDomainEvent>
{
    public Task Handle(MembershipExtendedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Membership with ID {MembershipId} has been marked as extended.", notification.MembershipId);

        // Здесь можно добавить дополнительные действия, например:
        // - Отправка уведомления администратору.
        // - Обновление статистики.

        return Task.CompletedTask;
    }
}