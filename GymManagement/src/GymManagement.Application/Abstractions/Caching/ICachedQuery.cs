using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Abstractions.Caching;

public interface ICachedQuery
{
    string CacheKey { get; }

    TimeSpan? Expiration { get; }
}

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery;