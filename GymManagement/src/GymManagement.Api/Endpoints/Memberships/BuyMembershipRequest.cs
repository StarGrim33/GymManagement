namespace GymManagement.Api.Endpoints.Memberships;

public sealed record BuyMembershipRequest(Guid UserId,
    Guid MembershipTypeId,
    Guid GymId);