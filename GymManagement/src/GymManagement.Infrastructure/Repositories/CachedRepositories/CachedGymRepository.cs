using System.Linq.Expressions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Gyms.QueryOptions;
using GymManagement.Infrastructure.Repositories.CacheKeys;
using Microsoft.Extensions.Caching.Hybrid;

namespace GymManagement.Infrastructure.Repositories.CachedRepositories;

public class CachedGymRepository(IGymRepository gymRepository, HybridCache hybridCache) : IGymRepository
{
    public async Task<Gym?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cachedGym = await hybridCache.GetOrCreateAsync(CachedKeys.GymById(id), async token =>
        {
            var gym = await gymRepository.GetByIdAsync(id, token);
            return gym;
        }, cancellationToken: cancellationToken);

        return cachedGym;
    }

    public async Task<Gym?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var cachedGym = await hybridCache.GetOrCreateAsync(CachedKeys.GymByName(name), async token =>
        {
            var gym = await gymRepository.GetByNameAsync(name, token);
            return gym;
        }, cancellationToken: cancellationToken);

        return cachedGym;
    }

    public async Task<Gym?> GetAsync(Expression<Func<Gym, bool>> predicate, GymQueryOptions gymQueryOptions, CancellationToken cancellationToken = default)
    {        
        // Не кэширую. Вызываю напрямую из основного репозитория
        return await gymRepository.GetAsync(predicate, gymQueryOptions, cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.GymsTotalCount;

        var totalCount = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var count = await gymRepository.GetTotalCountAsync(token);
            return count;
        }, cancellationToken: cancellationToken);

        return totalCount;
    }

    public async Task<List<Gym>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.GymsPaged(pageNumber, pageSize);

        var cachedGyms = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var gyms = await gymRepository.GetPagedAsync(pageNumber, pageSize, token);
            return gyms;
        }, cancellationToken: cancellationToken);

        return cachedGyms;
    }

    public async Task AddAsync(Gym gym, CancellationToken cancellationToken = default)
    {
        await gymRepository.AddAsync(gym, cancellationToken);
        await InvalidateCache(gym);
    }

    public async Task Update(Gym gym)
    {
        await gymRepository.Update(gym);
        await InvalidateCache(gym);
    }

    public async Task Delete(Gym gym)
    {
        await gymRepository.Delete(gym);
        await InvalidateCache(gym);
    }

    private async Task InvalidateCache(Gym gym)
    {
        await hybridCache.RemoveAsync(CachedKeys.GymById(gym.Id));
        await hybridCache.RemoveAsync(CachedKeys.GymByName(gym.Name.Value));
        await hybridCache.RemoveAsync(CachedKeys.GymsTotalCount);
    }
}