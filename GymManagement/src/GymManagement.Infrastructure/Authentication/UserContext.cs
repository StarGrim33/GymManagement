using GymManagement.Application.Abstractions.Authentication;
using GymManagement.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;

namespace GymManagement.Infrastructure.Authentication;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public string IdentityId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetIdentityId() ??
        throw new ApplicationException("User context is unavailable");
}