using GymManagement.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationDbContext dbContext) : Repository<User>(dbContext), IUserRepository
{
    public override async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<User>()
            .AsSplitQuery()
            .Include(u => u.Memberships)
            .Include( u => u.TrainingSessions)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<User>()
            .FirstOrDefaultAsync(u => u.Email == new Domain.Entities.Email(email), cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<User>().CountAsync(cancellationToken);
    }

    public async Task<List<User>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<User>()
            .OrderBy(u => u.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}