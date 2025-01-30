namespace GymManagement.Api.Controllers.Memberships;

public sealed record BuyMembershipRequest(Guid UserId,
    Guid MembershipTypeId,
    Guid GymId);