namespace GymManagement.Domain.Entities.Memberships;

public class MembershipPurchaseResult(Membership membership, bool isNewMembership)
{
    public Membership Membership { get; } = membership;

    public bool IsNewMembership { get; } = isNewMembership;
}