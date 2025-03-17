using GymManagement.Application.Abstractions.Caching;
using GymManagement.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Authorization;

internal sealed class AuthorizationService(
    ApplicationDbContext dbContext,
    ICacheService cacheService)
{
    public async Task<UserRolesResponse> GetRolesForUserAsync(string? identityId)
    {
        var cacheKey = $"auth:roles-{identityId}";

        var cachedRoles = await cacheService.GetAsync<UserRolesResponse>(cacheKey);

        if (cachedRoles is not null)
            return cachedRoles;

        var roles = await dbContext
            .Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            })
            .FirstAsync();

        await cacheService.SetAsync(cacheKey, roles);

        return roles;
    }
}