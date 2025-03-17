using System.Buffers;
using System.Text.Json;
using GymManagement.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace GymManagement.Infrastructure.Repositories.CachedRepositories;

internal sealed class CacheService(IDistributedCache cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
    {
        var bytes = await cache.GetAsync(cacheKey, cancellationToken);

        return bytes is null ? default : Deserialize<T>(bytes);
    }

    public Task SetAsync<T>(string cacheKey, T value, TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        var bytes = Serialize(value);

        return cache.SetAsync(cacheKey, bytes, CacheOptions.Create(expiration), cancellationToken);
    }

    public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        return cache.RemoveAsync(cacheKey, cancellationToken);
    }

    private static byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();

        using var writer = new Utf8JsonWriter(buffer);

        JsonSerializer.Serialize(writer, value);

        return buffer.WrittenSpan.ToArray();
    }

    private static T Deserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes)!;
    }
}

public static class CacheOptions
{
    private static DistributedCacheEntryOptions DefaultExpiration => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
    };

    public static DistributedCacheEntryOptions Create(TimeSpan? expiration)
    {
        return expiration is not null
            ? new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            }
            : DefaultExpiration;
    }
}