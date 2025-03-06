using GymManagement.Application.Abstractions.Email;
using GymManagement.Domain.Entities.Invoices.Events;
using MediatR;

namespace GymManagement.Application.Invoice;

internal sealed class InvoicePaidDomainEventHandler(IEmailService emailService) : INotificationHandler<InvoicePaidDomainEvent>
{
    public async Task Handle(InvoicePaidDomainEvent notification, CancellationToken cancellationToken)
    {
        await emailService.SendAsync(
            "user@example.com",
            "Invoice Paid",
            $"Your invoice with ID {notification.InvoiceId} has been successfully paid."
        );
    }
}