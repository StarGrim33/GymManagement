using GymManagement.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories;

internal abstract class Repository<T>(ApplicationDbContext dbContext)
    where T : Entity
{
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<T>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(T entity)
    {
        dbContext.Set<T>().Add(entity);
    }
}