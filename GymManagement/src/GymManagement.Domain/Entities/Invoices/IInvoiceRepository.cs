namespace GymManagement.Domain.Entities.Invoices;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default);

    Task Update(Invoice invoice);

    Task Delete(Invoice invoice);
}