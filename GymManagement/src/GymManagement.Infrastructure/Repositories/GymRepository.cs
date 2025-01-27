using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class GymRepository(ApplicationDbContext dbContext) 
    : Repository<Gym>(dbContext), IGymRepository
{
    public Task<Gym?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}