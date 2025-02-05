using GymManagement.Domain.Entities.Memberships.MembershipTypes;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class MembershipTypeRepository(ApplicationDbContext dbContext) 
    : Repository<MembershipType>(dbContext), IMembershipTypeRepository
{
    public async Task<MembershipType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<MembershipType>()
            .FirstOrDefaultAsync(mt => mt.Name == name, cancellationToken);
    }

    public override async Task<MembershipType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<MembershipType>()
            .FirstOrDefaultAsync(mt => mt.Id == id, cancellationToken);
    }
}