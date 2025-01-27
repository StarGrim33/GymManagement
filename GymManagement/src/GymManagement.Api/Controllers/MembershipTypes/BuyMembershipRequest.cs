namespace GymManagement.Api.Controllers.MembershipTypes;

public sealed record BuyMembershipRequest(Guid UserId,
    Guid MembershipTypeId,
    Guid GymId);