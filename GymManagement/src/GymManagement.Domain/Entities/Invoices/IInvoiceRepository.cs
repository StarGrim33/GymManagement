namespace GymManagement.Domain.Entities.Invoices;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(Invoice invoice);
}