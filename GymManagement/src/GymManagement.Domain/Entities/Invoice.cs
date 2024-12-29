using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities;

public sealed class Invoice(Guid id) : Entity(id)
{
    public DateTime InvoiceDate { get; private set; }

    public decimal Amount { get; private set; }

    public PaymentStatus PaymentStatus { get; private set; }

    public DateTime PaymentDate { get; private set; }

    public Membership Membership { get; private set; }
}