using GymManagement.Domain.Entities.Memberships.MembershipTypes;

namespace GymManagement.Application.Memberships.GetMembership;

public sealed class MembershipResponse
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public Guid MembershipId { get; init; }

    public int Status { get; init; }

    public decimal PriceAmount { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public MembershipType? MembershipType { get; init; }
}