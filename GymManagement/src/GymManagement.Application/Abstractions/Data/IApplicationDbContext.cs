using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    // Write operations
    Task AddEntityAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
    void UpdateEntity<T>(T entity) where T : class;
    void RemoveEntity<T>(T entity) where T : class;

    // Query operations
    Task<T?> FindAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : class;
    Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken = default) where T : class;

    // Specific queries
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Gym?> GetGymByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Gym?> GetGymWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<User>> GetPagedUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}