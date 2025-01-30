using GymManagement.Domain.Entities.Invoices;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class InvoiceRepository(ApplicationDbContext dbContext) 
    : Repository<Invoice>(dbContext), IInvoiceRepository
{
    
}