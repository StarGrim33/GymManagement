using GymManagement.Domain.Entities.Gyms;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class GymRepository(ApplicationDbContext dbContext) 
    : Repository<Gym>(dbContext), IGymRepository
{
    public async Task<Gym?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Gym>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(g => g.GymAmenities)
            .Include(g => g.Trainers)
            .Include(g => g.Equipment)
            .Include(g => g.Memberships)
            .Include(g => g.TrainingSessions)
            .FirstOrDefaultAsync(g => g.Name.Value == name, cancellationToken);
    }

    public override async Task<Gym?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Gym>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(g => g.GymAmenities)
            .Include(g => g.Trainers)
            .Include(g => g.Equipment)
            .Include(g => g.Memberships)
            .Include(g => g.TrainingSessions)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }
}