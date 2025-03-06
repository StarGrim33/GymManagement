using GymManagement.Application.Abstractions.Authentication;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Users.Errors;

namespace GymManagement.Application.Loging;

internal sealed class LogInUserCommandHandler(IJwtService jwtService)
    : ICommandHandler<LogInUserCommand, AccessTokenResponse>
{
    public async Task<Result<AccessTokenResponse>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
    {
        var result = await jwtService.GetAccessTokenAsync(
            request.Email,
            request.Password,
            cancellationToken);

        return result.IsFailure ? Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials) : new AccessTokenResponse(result.Value);
    }
}