namespace GymManagement.Infrastructure.Authentication;

public sealed class AuthenticationOptions
{
    public string Audience { get; init; } = string.Empty;

    public string MetadataUrl { get; init; } = string.Empty;

    public bool RequireHttpsMetadata { get; init; } = false;

    public string Issuer { get; set; } = string.Empty;
}