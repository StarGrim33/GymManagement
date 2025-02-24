using GymManagement.Domain.Entities.Memberships;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class MembershipRepository(ApplicationDbContext dbContext) 
    : Repository<Membership>(dbContext), IMembershipRepository
{
    public async Task<MembershipDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Membership>()
            .Where(m => m.Id == id)
            .Select(m => new MembershipDto
            {
                Id = m.Id,
                MembershipType = m.MembershipType.Name,
                Price = m.PriceAmount,
                StartDate = m.StartDate,
                EndDate = m.EndDate,
                IsActive = m.IsActive,
                MembershipStatus = m.MembershipStatus,
                MembershipTypeId = m.MembershipTypeId,
                GymId = m.GymId,
                MembershipTypeDuration = m.MembershipType.Duration,
                MembershipTypeName = m.MembershipType.Name,
                UserId = m.UserId
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Membership>().CountAsync(cancellationToken);
    }

    public async Task<List<MembershipDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Membership>()
            .Include(m => m.MembershipType)
            .OrderBy(m => m.StartDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MembershipDto
            {
                Id = m.Id,
                MembershipType = m.MembershipType.Name,
                Price = m.PriceAmount,
                StartDate = m.StartDate,
                EndDate = m.EndDate,
                IsActive = m.IsActive,
                MembershipStatus = m.MembershipStatus,
                MembershipTypeId = m.MembershipTypeId,
                GymId = m.GymId,
                MembershipTypeDuration = m.MembershipType.Duration,
                MembershipTypeName = m.MembershipType.Name,
                UserId = m.UserId
            })
            .ToListAsync(cancellationToken);
    }
}