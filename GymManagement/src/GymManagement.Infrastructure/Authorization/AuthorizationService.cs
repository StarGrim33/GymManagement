using GymManagement.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Authorization;

internal sealed class AuthorizationService(ApplicationDbContext dbContext)
{
    public async Task<UserRolesResponse> GetRolesForUserAsync(string? identityId)
    {
        var roles = await dbContext
            .Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            })
            .FirstAsync();

        return roles;
    }
}