using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Memberships;

namespace GymManagement.Domain.Entities.Invoices;

public sealed class Invoice : Entity
{
    private Invoice(
        Guid id,
        DateTime invoiceDate, 
        decimal amount, 
        PaymentStatus paymentStatus, 
        DateTime? paymentDate, 
        Membership membership) 
        : base(id)
    {
        InvoiceDate = invoiceDate;
        Amount = amount;
        PaymentStatus = paymentStatus;
        PaymentDate = paymentDate;
        Membership = membership;
    }

    public DateTime InvoiceDate { get; private set; }

    public decimal Amount { get; private set; }

    public PaymentStatus PaymentStatus { get; private set; }

    public DateTime? PaymentDate { get; private set; }

    public Membership Membership { get; private set; }

    public static Invoice Create(
        Guid id,
        DateTime invoiceDate,
        decimal amount,
        PaymentStatus paymentStatus,
        DateTime? paymentDate,
        Membership membership)
    {
        return new Invoice(
            id,
            invoiceDate,
            amount,
            paymentStatus,
            paymentDate,
            membership);
    }

    public void MarkAsPaid(DateTime paymentDate)
    {
        PaymentStatus = PaymentStatus.Paid;
        PaymentDate = paymentDate;

        Membership.Activate();
    }
}