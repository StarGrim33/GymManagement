using System.Linq.Expressions;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Gyms.QueryOptions;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class GymRepository(ApplicationDbContext dbContext) 
    : Repository<Gym>(dbContext), IGymRepository
{
    public async Task<Gym?> GetEntityByIdAsync(Guid id, CancellationToken cancellationToken = default)
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

    public async Task<GymDto?> GetByNameAsync(string? name, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Gym>()
            .Where(g => g.Name.Value == name)
            .Select(g => new GymDto
            {
                Id = g.Id,
                Name = g.Name.Value,
                Description = g.Description.Value,
                Address = new AddressDto
                {
                    Street = g.Address.Street,
                    City = g.Address.City,
                    ZipCode = g.Address.ZipCode
                },
                Schedule = g.Schedule.Value
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<GymDto?> GetAsync(Expression<Func<Gym, bool>> predicate, GymQueryOptions gymQueryOptions, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<Gym>().AsQueryable();

        if (gymQueryOptions.AsNoTracking)
            query = query.AsNoTracking();

        return await query
            .Where(predicate)
            .Select(g => new GymDto
            {
                Id = g.Id,
                Name = g.Name.Value,
                Description = g.Description.Value,
                Address = new AddressDto
                {
                    Street = g.Address.Street,
                    City = g.Address.City,
                    ZipCode = g.Address.ZipCode
                },
                Schedule = g.Schedule.Value
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Gym>().CountAsync(cancellationToken);
    }

    public async Task<List<GymDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Gym>()
            .OrderBy(g => g.Name.Value)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(g => new GymDto
            {
                Id = g.Id,
                Name = g.Name.Value,
                Description = g.Description.Value,
                Address = new AddressDto
                {
                    Street = g.Address.Street,
                    City = g.Address.City,
                    ZipCode = g.Address.ZipCode
                },
                Schedule = g.Schedule.Value
            }).ToListAsync(cancellationToken);
    }

    public async Task<GymDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Gym>()
            .Where(g => g.Id == id)
            .Select(g => new GymDto
            {
                Id = g.Id,
                Name = g.Name.Value,
                Description = g.Description.Value,
                Address = new AddressDto
                {
                    Street = g.Address.Street,
                    City = g.Address.City,
                    ZipCode = g.Address.ZipCode
                },
                Schedule = g.Schedule.Value
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}