using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Application.Common;

namespace GymManagement.Application.Memberships.GetMembership.GetAllMemberships;

public record GetAllMembershipsQuery(int PageNumber, int PageSize) : IQuery<PaginatedList<MembershipResponse>>;