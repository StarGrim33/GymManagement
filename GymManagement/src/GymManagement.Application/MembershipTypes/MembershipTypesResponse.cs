namespace GymManagement.Application.MembershipTypes;

public sealed class MembershipTypesResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public TimeSpan Duration { get; init; }

    public decimal Price { get; init; }
}