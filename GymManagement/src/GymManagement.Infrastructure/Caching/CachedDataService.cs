using GymManagement.Application.Abstractions.Data;
using GymManagement.Domain.Entities.Gyms;
using Microsoft.Extensions.Caching.Hybrid;

namespace GymManagement.Infrastructure.Caching;

public interface ICachedDataService
{
    Task<Gym?> GetGymByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task InvalidateGymCacheAsync(Guid gymId);
}

internal class CachedDataService(ApplicationDbContext dbContext, HybridCache cache) : ICachedDataService
{
    public async Task<Gym?> GetGymByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await cache.GetOrCreateAsync($"gym-{id}", async token =>
        {
            return await dbContext.GetGymWithDetailsAsync(id, token);
        }, cancellationToken: cancellationToken);
    }

    public async Task InvalidateGymCacheAsync(Guid gymId)
    {
        await cache.RemoveAsync($"gym-{gymId}");
    }
}