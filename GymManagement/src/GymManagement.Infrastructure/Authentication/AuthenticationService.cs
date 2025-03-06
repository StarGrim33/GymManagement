using System.Net.Http.Json;
using GymManagement.Application.Abstractions.Authentication;
using GymManagement.Domain.Entities.Users;
using GymManagement.Infrastructure.Authentication.Models;

namespace GymManagement.Infrastructure.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
    private const string PasswordCredentialType = "password";

    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> RegisterAsync(
        User user, 
        string password,
        CancellationToken cancellationToken = default)
    {
        var userRepresentationModel = UserRepresentationModel.FromUser(user);

        userRepresentationModel.Credentials =
        [
            new CredentialRepresentationModel
            {
                Value = password,
                Temporary = false,
                Type = PasswordCredentialType
            }
        ];

        var response = await _httpClient.PostAsJsonAsync(
            "users",
            userRepresentationModel,
            cancellationToken);

        return ExtractIdentityFromLocationHeader(response);
    }

    private static string ExtractIdentityFromLocationHeader(
        HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header can`t be null");
        }

        var userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName, 
            StringComparison.InvariantCultureIgnoreCase);

        var userIdentityId = locationHeader[(userSegmentValueIndex + usersSegmentName.Length)..];

        return userIdentityId;
    }
}