using GymManagement.Domain.Entities.Memberships;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class MembershipRepository(ApplicationDbContext dbContext) 
    : Repository<Membership>(dbContext), IMembershipRepository
{
    public override async Task<Membership?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Membership>()
            .FirstOrDefaultAsync(mt => mt.Id == id, cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Membership>().CountAsync(cancellationToken);
    }

    public async Task<List<Membership>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Membership>()
            .Include(m => m.MembershipType)
            .OrderBy(m => m.StartDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}