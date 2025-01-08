using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Memberships.BuyMembership;

public record BuyMembershipCommand(
    Guid UserId,
    Guid MembershipTypeId,
    Guid GymId) : ICommand<Guid>;
