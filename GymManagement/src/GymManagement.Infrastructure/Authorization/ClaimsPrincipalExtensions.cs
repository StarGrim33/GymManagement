﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GymManagement.Infrastructure.Authorization;

internal static class ClaimsPrincipalExtensions
{
    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
               throw new ApplicationException("User identity is unavailable");
    }

    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(userId, out var parsedUserId) 
            ? parsedUserId : throw new ApplicationException("User identity is invalid");
    }
}