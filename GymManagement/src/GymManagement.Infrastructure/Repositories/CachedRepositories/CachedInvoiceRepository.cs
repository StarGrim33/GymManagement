using GymManagement.Domain.Entities.Invoices;
using GymManagement.Infrastructure.Repositories.CacheKeys;
using Microsoft.Extensions.Caching.Hybrid;

namespace GymManagement.Infrastructure.Repositories.CachedRepositories;

public class CachedInvoiceRepository(IInvoiceRepository invoiceRepository, HybridCache hybridCache) : IInvoiceRepository
{
    public async Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.InvoiceById(id);

        var cachedInvoice = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var invoice = await invoiceRepository.GetByIdAsync(id, token);
            return invoice;
        }, cancellationToken: cancellationToken);

        return cachedInvoice;
    }

    public async Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        await invoiceRepository.AddAsync(invoice, cancellationToken);
        await InvalidateCache(invoice);
    }

    public async Task Update(Invoice invoice)
    {
        await invoiceRepository.Update(invoice);
        await InvalidateCache(invoice);
    }

    public async Task Delete(Invoice invoice)
    {
        await invoiceRepository.Delete(invoice);
        await InvalidateCache(invoice);
    }

    private async Task InvalidateCache(Invoice invoice)
    {
        await hybridCache.RemoveAsync(CachedKeys.InvoiceById(invoice.Id));
    }
}