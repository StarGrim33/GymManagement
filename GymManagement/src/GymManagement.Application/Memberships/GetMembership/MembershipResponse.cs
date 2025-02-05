using GymManagement.Application.MembershipTypes;

namespace GymManagement.Application.Memberships.GetMembership;

public sealed class MembershipResponse
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public Guid GymId { get; init; }

    public string? Name { get; init; }

    public decimal PriceAmount { get; init; }

    public int Status { get; init; }

    public bool IsActive { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public MembershipTypesResponse? MembershipType { get; init; }
}