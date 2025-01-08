using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.MembershipTypes.GetMembershipTypes;

public sealed record GetMembershipTypesQuery() : IQuery<MembershipTypesResponse>;