using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Users.GetUser;

public record GetUserQuery(Guid UserId) : IQuery<UserResponse>;