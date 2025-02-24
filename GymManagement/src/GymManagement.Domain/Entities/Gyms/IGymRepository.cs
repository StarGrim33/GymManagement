using System.Linq.Expressions;
using GymManagement.Domain.Entities.Gyms.QueryOptions;

namespace GymManagement.Domain.Entities.Gyms;

public interface IGymRepository
{
    Task<GymDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Gym?> GetEntityByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<GymDto?> GetByNameAsync(string? name, CancellationToken cancellationToken = default);

    Task<GymDto?> GetAsync(Expression<Func<Gym, bool>> predicate, GymQueryOptions gymQueryOptions,
        CancellationToken cancellationToken = default);

    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

    Task<List<GymDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    Task AddAsync(Gym gym, CancellationToken cancellationToken = default);

    Task Update(Gym gym);

    Task Delete(Gym gym);
}