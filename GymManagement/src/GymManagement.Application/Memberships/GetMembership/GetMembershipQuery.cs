using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Memberships.GetMembership;

public sealed record GetMembershipQuery(Guid MembershipId) : IQuery<MembershipResponse>;