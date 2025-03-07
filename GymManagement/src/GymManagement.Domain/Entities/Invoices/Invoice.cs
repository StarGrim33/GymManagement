﻿using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Invoices.Events;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Memberships.Errors;

namespace GymManagement.Domain.Entities.Invoices;

public sealed class Invoice : Entity
{
    private Invoice(
        Guid id,
        DateTime invoiceDate, 
        decimal amount, 
        PaymentStatus paymentStatus, 
        DateTime? paymentDate, 
        Membership membership, Guid membershipId) 
        : base(id)
    {
        InvoiceDate = invoiceDate;
        Amount = amount;
        PaymentStatus = paymentStatus;
        PaymentDate = paymentDate;
        Membership = membership;
        MembershipId = membershipId;
    }

    private Invoice()
    {
    }

    public DateTime InvoiceDate { get; private set; }

    public decimal Amount { get; private set; }

    public PaymentStatus PaymentStatus { get; private set; }

    public DateTime? PaymentDate { get; private set; }

    public Membership Membership { get; private set; }

    public Guid MembershipId { get; private set; }

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
            membership,
            membership.Id);
    }

    public Result MarkAsPaid(DateTime paymentDate)
    {
        if (PaymentStatus is not PaymentStatus.Paid)
            return Result.Failure(MembershipErrors.NotActivated);

        PaymentStatus = PaymentStatus.Paid;
        PaymentDate = paymentDate;

        Membership.MarkAsPaid();
        RaiseDomainEvent(new InvoicePaidDomainEvent(Id));
        return Result.Success();
    }
}