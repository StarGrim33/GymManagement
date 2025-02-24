using GymManagement.Domain.Entities.Trainers;
using GymManagement.Infrastructure.Repositories.CacheKeys;
using Microsoft.Extensions.Caching.Hybrid;

namespace GymManagement.Infrastructure.Repositories.CachedRepositories;

public class CachedTrainerRepository(ITrainerRepository trainerRepository, HybridCache hybridCache) : ITrainerRepository
{
    private async Task InvalidateCache(Trainer trainer)
    {
        await hybridCache.RemoveAsync(CachedKeys.TrainerById(trainer.Id));
        await hybridCache.RemoveAsync(CachedKeys.TrainerByName(trainer.FirstName.Value));
    }

    public async Task<Trainer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.TrainerById(id);

        var cachedTrainer = await hybridCache.GetOrCreateAsync(cacheKey, async token =>
        {
            var trainer = await trainerRepository.GetByIdAsync(id, token);
            return trainer;
        }, cancellationToken: cancellationToken);

        return cachedTrainer;
    }

    public async Task<List<Trainer>?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await trainerRepository.GetByNameAsync(name, cancellationToken);
    }

    public async Task<Trainer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var cacheKey = CachedKeys.TrainerByEmail(email);

        var cachedTrainer = await hybridCache.GetOrCreateAsync(cacheKey, async token => 
            await trainerRepository.GetByEmailAsync(email, token), cancellationToken: cancellationToken);

        return cachedTrainer;
    }

    public async Task AddAsync(Trainer trainer, CancellationToken cancellationToken = default)
    {
        await trainerRepository.AddAsync(trainer, cancellationToken);
        await InvalidateCache(trainer);
    }

    public async Task Update(Trainer trainer)
    {
        await trainerRepository.Update(trainer);
        await InvalidateCache(trainer);
    }

    public async Task Delete(Trainer trainer)
    {
        await trainerRepository.Delete(trainer);
        await InvalidateCache(trainer);
    }
}