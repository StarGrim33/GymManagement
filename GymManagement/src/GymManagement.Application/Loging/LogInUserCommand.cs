using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Loging;

public sealed record LogInUserCommand(string Email, string Password) : ICommand<AccessTokenResponse>;