using System.Linq.Expressions;
using GymManagement.Domain.Entities.Gyms.QueryOptions;

namespace GymManagement.Domain.Entities.Gyms;

public interface IGymRepository
{
    Task<Gym?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Gym?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<Gym?> GetAsync(Expression<Func<Gym, bool>> predicate, GymQueryOptions gymQueryOptions,
        CancellationToken cancellationToken = default);

    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

    Task<List<Gym>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    Task AddAsync(Gym gym, CancellationToken cancellationToken = default);

    Task Update(Gym gym);

    Task Delete(Gym gym);
}