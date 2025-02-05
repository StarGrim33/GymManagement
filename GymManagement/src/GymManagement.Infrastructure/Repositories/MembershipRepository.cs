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
}