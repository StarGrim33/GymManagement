using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Application.Common;

namespace GymManagement.Application.Users.GetUser.GetAllUsers
{
    public sealed record GetAllUsersQuery(int PageNumber, int PageSize) : IQuery<PaginatedList<UserResponse>>;
}
