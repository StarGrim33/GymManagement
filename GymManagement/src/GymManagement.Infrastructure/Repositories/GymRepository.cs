using System.Linq.Expressions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Gyms.QueryOptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class GymRepository(ApplicationDbContext dbContext) 
    : Repository<Gym>(dbContext), IGymRepository
{
    public async Task<Gym?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Gym>()
            .AsSplitQuery()
            .Include(g => g.GymAmenities)
            .Include(g => g.Trainers)
            .Include(g => g.Equipment)
            .Include(g => g.Memberships)
            .Include(g => g.TrainingSessions)
            .FirstOrDefaultAsync(g => g.Name.Value == name, cancellationToken);
    }

    public async Task<Gym?> GetAsync(Expression<Func<Gym, bool>> predicate, GymQueryOptions gymQueryOptions, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<Gym>().AsQueryable();

        if (gymQueryOptions.AsNoTracking)
            query = query.AsNoTracking();

        if (gymQueryOptions.IncludeAmenities)
            query = query.Include(g => g.GymAmenities);

        if (gymQueryOptions.IncludeTrainers)
            query = query.Include(g => g.Trainers);

        if (gymQueryOptions.IncludeEquipment)
            query = query.Include(g => g.Equipment);

        if (gymQueryOptions.IncludeMemberships)
            query = query.Include(g => g.Memberships);

        if (gymQueryOptions.IncludeTrainingSessions)
            query = query.Include(g => g.TrainingSessions);

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Gym>().CountAsync(cancellationToken);
    }

    public async Task<List<Gym>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Gym>()
            .OrderBy(g => g.Name) // Сортировка по имени (или другому полю)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Gym?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Gym>()
            .AsSplitQuery()
            .Include(g => g.GymAmenities)
            .Include(g => g.Trainers)
            .Include(g => g.Equipment)
            .Include(g => g.Memberships)
            .Include(g => g.TrainingSessions)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }
}