using GymManagement.Domain.Entities.Invoices;

namespace GymManagement.Domain.Entities.Memberships;

public class MembershipDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public Guid GymId { get; init; }

    public Guid MembershipTypeId { get; init; }

    public string? MembershipType { get; init; }

    public string? MembershipTypeName { get; init; }

    public TimeSpan MembershipTypeDuration { get; init; }

    public decimal Price { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public bool IsActive { get; init; }

    public MembershipStatus MembershipStatus { get; init; }

    public List<Invoice>? Invoices { get; init; }
}