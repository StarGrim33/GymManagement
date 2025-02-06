using GymManagement.Domain.Entities.Memberships.MembershipTypes;
using GymManagement.Infrastructure.Repositories.CacheKeys;
using Microsoft.Extensions.Caching.Hybrid;

namespace GymManagement.Infrastructure.Repositories.CachedRepositories;

public class CachedMembershipTypeRepository(IMembershipTypeRepository membershipTypeRepository, HybridCache hybridCache) : IMembershipTypeRepository
{
    public async Task<MembershipType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.MembershipTypeById(id);

        var cachedMembershipType = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var membershipType = await membershipTypeRepository.GetByIdAsync(id, token);
            return membershipType;
        }, cancellationToken: cancellationToken);

        return cachedMembershipType;
    }

    public async Task<MembershipType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.MembershipTypeByName(name);

        var cachedMembershipType = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var membershipType = await membershipTypeRepository.GetByNameAsync(name, token);
            return membershipType;
        }, cancellationToken: cancellationToken);

        return cachedMembershipType;
    }

    public async Task AddAsync(MembershipType membershipType, CancellationToken cancellationToken = default)
    {
        await membershipTypeRepository.AddAsync(membershipType, cancellationToken);
        await InvalidateCache(membershipType);
    }

    public async Task Update(MembershipType membershipType)
    {
        await membershipTypeRepository.Update(membershipType);
        await InvalidateCache(membershipType);
    }

    public async Task Delete(MembershipType membershipType)
    {
        await membershipTypeRepository.Delete(membershipType);
        await InvalidateCache(membershipType);
    }

    private async Task InvalidateCache(MembershipType membershipType)
    {
        await hybridCache.RemoveAsync(CachedKeys.MembershipTypeById(membershipType.Id));
        await hybridCache.RemoveAsync(CachedKeys.MembershipTypeByName(membershipType.Name));
    }
}