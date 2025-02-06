using GymManagement.Domain.Entities.Users;
using GymManagement.Infrastructure.Repositories.CacheKeys;
using Microsoft.Extensions.Caching.Hybrid;

namespace GymManagement.Infrastructure.Repositories.CachedRepositories;

public class CachedUserRepository(IUserRepository userRepository, HybridCache hybridCache) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.UserById(id);

        var cachedUser = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var user = await userRepository.GetByIdAsync(id, token);
            return user;
        }, cancellationToken: cancellationToken);

        return cachedUser;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.UserByEmail(email);

        var cachedUser = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var user = await userRepository.GetByEmailAsync(email, token);
            return user;
        }, cancellationToken: cancellationToken);

        return cachedUser;
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.UsersTotalCount;

        var cachedCount = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var count = await userRepository.GetTotalCountAsync(token);
            return count;
        }, cancellationToken: cancellationToken);

        return cachedCount;
    }

    public async Task<List<User>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{CachedKeys.UsersPaged(pageNumber, pageSize)}";

        var cachedUsers = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var users = await userRepository.GetPagedAsync(pageNumber, pageSize, token);
            return users;
        }, cancellationToken: cancellationToken);

        return cachedUsers;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await userRepository.AddAsync(user, cancellationToken);
        await InvalidateCache(user);
    }

    public async Task Update(User user)
    {
        await userRepository.Update(user);
        await InvalidateCache(user);
    }

    public async Task Delete(User user)
    {
        await userRepository.Delete(user);
        await InvalidateCache(user);
    }

    private async Task InvalidateCache(User user)
    {
        await hybridCache.RemoveAsync(CachedKeys.UserById(user.Id));
        await hybridCache.RemoveAsync(CachedKeys.UserByEmail(user.Email.Value));
        await hybridCache.RemoveAsync(CachedKeys.UsersTotalCount);
    }
}