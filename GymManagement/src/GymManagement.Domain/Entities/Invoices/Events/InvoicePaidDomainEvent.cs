using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Invoices.Events;

public sealed record InvoicePaidDomainEvent(Guid InvoiceId) : IDomainEvent;