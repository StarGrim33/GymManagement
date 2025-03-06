using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Memberships.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GymManagement.Application.Memberships.BuyMembership;

internal sealed class MembershipPurchasedDomainEventHandler(ILogger<MembershipPurchasedDomainEventHandler> logger,
    IMembershipRepository membershipRepository)
    : INotificationHandler<MembershipPaidDomainEvent>
{
    public async Task Handle(MembershipPaidDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Membership with ID {MembershipId} has been marked as pending payment. You have 10 minutes to pay it.", notification.MembershipId);

        var membership = await membershipRepository.GetByIdAsync(notification.MembershipId, cancellationToken); 
        
        if (membership is null)
        {
            logger.LogError("Membership is not found");
        }

        var invoice = membership?.Invoices?.FirstOrDefault();

        invoice?.MarkAsPaid(DateTime.UtcNow);

        logger.LogInformation("Invoice {invoiceId} has marked as paid", invoice?.Id);
        // Здесь можно добавить дополнительные действия, например:
        // - Отправка уведомления администратору.
        // - Обновление статистики.
    }
}