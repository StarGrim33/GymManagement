using GymManagement.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationDbContext dbContext) : Repository<User>(dbContext), IUserRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public override async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<User>()
            .AsSplitQuery()
            .Include(u => u.Memberships)
            .Include( u => u.TrainingSessions)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<User>()
            .FirstOrDefaultAsync(u => u.Email == new Domain.Entities.Email(email), cancellationToken);
    }
}