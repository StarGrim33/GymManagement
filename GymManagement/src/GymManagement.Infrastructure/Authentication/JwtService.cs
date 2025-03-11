using GymManagement.Application.Abstractions.Authentication;
using GymManagement.Domain.Abstractions;
using GymManagement.Infrastructure.Authentication.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace GymManagement.Infrastructure.Authentication;

internal sealed class JwtService(
    HttpClient httpClient, 
    IOptions<KeycloakOptions> keycloakOptions)
    : IJwtService
{
    private static readonly Error AuthenticationFailed = new(
        "Keycloak.AuthenticationFailed",
        "Failed to acquire access token to do authentication failure");

    public async Task<Result<string>> GetAccessTokenAsync(
        string email, 
        string password, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var authRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", keycloakOptions.Value.AuthClientId),
                new("client_secret", keycloakOptions.Value.AuthClientSecret),
                new("scope", "openid email"),
                new("grant_type", "password"),
                new("username", email),
                new("password", password)
            };

            var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

            var response = await httpClient.PostAsync("", authorizationRequestContent, cancellationToken);

            response.EnsureSuccessStatusCode();

            var authorizationToken = await response.Content.
                ReadFromJsonAsync<AuthorizationToken>(cancellationToken: cancellationToken);

            return authorizationToken?.AccessToken 
                   ?? Result.Failure<string>(AuthenticationFailed);
        }
        catch (HttpRequestException)
        {
            return Result.Failure<string>(AuthenticationFailed);
        }
    }
}