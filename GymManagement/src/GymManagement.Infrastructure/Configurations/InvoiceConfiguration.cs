using GymManagement.Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Configurations;

internal sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("invoices");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.InvoiceDate)
            .HasColumnName("invoice_date")
            .IsRequired();

        builder.Property(i => i.Amount)
            .HasColumnName("amount")
            .HasPrecision(18,2)
            .IsRequired();

        builder.Property(i => i.PaymentStatus)
            .HasColumnName("payment_status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(i => i.PaymentDate)
            .HasColumnName("payment_date");

        builder.HasOne(i => i.Membership)
            .WithMany(m => m.Invoices)
            .HasForeignKey(i => i.MembershipId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}