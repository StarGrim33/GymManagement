using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Users.GetLoggedInUser
{
    public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;
}
