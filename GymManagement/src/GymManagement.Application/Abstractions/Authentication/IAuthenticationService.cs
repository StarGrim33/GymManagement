using GymManagement.Domain.Entities.Users;

namespace GymManagement.Application.Abstractions.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(
        User user, 
        string password, 
        CancellationToken cancellationToken = default);
}