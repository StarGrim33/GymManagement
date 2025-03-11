using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure.Authorization;

internal sealed class CustomClaimsTransformation(IServiceProvider serviceProvider) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
            principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
            return principal;

        using var scope = serviceProvider.CreateScope();

        var authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();

        var identityId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var userRoles = await authorizationService.GetRolesForUserAsync(identityId);

        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.Id.ToString()));

        foreach (var role in userRoles.Roles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
        }

        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}