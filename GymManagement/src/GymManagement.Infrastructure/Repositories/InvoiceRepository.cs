using GymManagement.Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class InvoiceRepository(ApplicationDbContext dbContext) 
    : Repository<Invoice>(dbContext), IInvoiceRepository
{
    public async Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Invoice>()
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }
}