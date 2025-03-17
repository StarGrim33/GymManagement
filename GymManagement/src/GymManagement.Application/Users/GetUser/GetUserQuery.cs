using GymManagement.Application.Abstractions.Caching;

namespace GymManagement.Application.Users.GetUser;

public sealed record GetUserQuery(Guid UserId) : ICachedQuery<UserResponse>
{
    public string CacheKey => $"users-{UserId}";

    public TimeSpan? Expiration => null ;
}