using GymManagement.Domain.Abstractions;

namespace GymManagement.Infrastructure.Repositories;

internal abstract class Repository<T>(ApplicationDbContext dbContext)
    where T : Entity
{
    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<T>().AddAsync(entity, cancellationToken);
    }

    public virtual Task Update(T entity)
    {
        dbContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task Delete(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
}