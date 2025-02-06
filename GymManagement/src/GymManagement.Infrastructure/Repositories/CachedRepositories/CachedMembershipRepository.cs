using GymManagement.Domain.Entities.Memberships;
using GymManagement.Infrastructure.Repositories.CacheKeys;
using Microsoft.Extensions.Caching.Hybrid;

namespace GymManagement.Infrastructure.Repositories.CachedRepositories;

public class CachedMembershipRepository(IMembershipRepository membershipRepository, HybridCache hybridCache) : IMembershipRepository
{
    public async Task<Membership?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.MembershipById(id);

        var cachedMembership = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var membership = await membershipRepository.GetByIdAsync(id, token);
            return membership;
        }, cancellationToken: cancellationToken);

        return cachedMembership;
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.MembershipsTotalCount;

        var totalCount = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var count = await membershipRepository.GetTotalCountAsync(token);
            return count;
        }, cancellationToken: cancellationToken);

        return totalCount;
    }

    public async Task<List<Membership>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{CachedKeys.MembershipsPaged(pageNumber, pageSize)}";

        var cachedMemberships = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var memberships = await membershipRepository.GetPagedAsync(pageNumber, pageSize, token);
            return memberships;
        }, cancellationToken: cancellationToken);

        return cachedMemberships;
    }

    public async Task AddAsync(Membership membership, CancellationToken cancellationToken = default)
    {
        await membershipRepository.AddAsync(membership, cancellationToken);
        await InvalidateCache(membership);
    }

    public async Task Update(Membership membership)
    {
        await membershipRepository.Update(membership);
        await InvalidateCache(membership);
    }

    public async Task Delete(Membership membership)
    {
        await membershipRepository.Delete(membership);
        await InvalidateCache(membership);
    }

    private async Task InvalidateCache(Membership membership)
    {
        await hybridCache.RemoveAsync(CachedKeys.MembershipById(membership.Id));
        await hybridCache.RemoveAsync(CachedKeys.MembershipsTotalCount);
    }
}