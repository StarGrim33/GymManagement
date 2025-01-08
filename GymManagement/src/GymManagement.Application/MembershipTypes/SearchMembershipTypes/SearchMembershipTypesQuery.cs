using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.MembershipTypes.SearchMembershipTypes;

public sealed record SearchMembershipTypesQuery(string Name) 
    : IQuery<IReadOnlyList<MembershipTypesResponse>>;