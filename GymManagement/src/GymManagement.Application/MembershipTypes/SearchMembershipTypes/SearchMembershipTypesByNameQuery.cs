using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.MembershipTypes.SearchMembershipTypes;

public sealed record SearchMembershipTypesByNameQuery(string Name) 
    : IQuery<MembershipTypesResponse>;