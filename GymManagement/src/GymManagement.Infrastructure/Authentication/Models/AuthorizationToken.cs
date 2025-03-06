using System.Text.Json.Serialization;

namespace GymManagement.Infrastructure.Authentication.Models;

public class AuthorizationToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;
}