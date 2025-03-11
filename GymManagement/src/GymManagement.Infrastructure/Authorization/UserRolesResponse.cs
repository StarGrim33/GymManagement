using GymManagement.Domain.Entities.Users;

namespace GymManagement.Infrastructure.Authorization;

public sealed class UserRolesResponse
{
    public Guid Id { get; init; }

    public List<Role> Roles { get; init; } = [];
}