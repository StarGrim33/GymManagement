using GymManagement.Application.Abstractions.Caching;
using GymManagement.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GymManagement.Application.Abstractions.Behaviors;

internal sealed class QueryCachingBehavior<TRequest, TResponse>(
    ICacheService cacheService,
    ILogger<QueryCachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var cachedResult = await cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);

        var name = typeof(TRequest).Name;

        if (cachedResult is not null)
        {
            logger.LogInformation("Cache hit for {Query}", name);
            return cachedResult;
        }

        logger.LogInformation("Cache miss for {Query}", name);

        var result = await next();

        if (result.IsSuccess)
            await cacheService.SetAsync(request.CacheKey, result, request.Expiration, cancellationToken);

        return result;
    }
}