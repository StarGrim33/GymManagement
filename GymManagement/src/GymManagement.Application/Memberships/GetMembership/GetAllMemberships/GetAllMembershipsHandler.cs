using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Application.Common;
using GymManagement.Application.MembershipTypes;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Memberships.Errors;

namespace GymManagement.Application.Memberships.GetMembership.GetAllMemberships;

internal sealed class GetAllMembershipsHandler(IMembershipRepository repository)
    : IQueryHandler<GetAllMembershipsQuery, PaginatedList<MembershipResponse>>
{
    public async Task<Result<PaginatedList<MembershipResponse>>> Handle(GetAllMembershipsQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await repository.GetTotalCountAsync(cancellationToken);

        var memberships = await repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        if (memberships.Count == 0)
            return Result.Failure<PaginatedList<MembershipResponse>>(MembershipErrors.NotFound);

        var membershipResponses = memberships.Select(m => new MembershipResponse
        {
            Id = m.Id,
            Name = m.MembershipType, 
            PriceAmount = m.Price,
            StartDate = m.StartDate,
            EndDate = m.EndDate,
            IsActive = m.IsActive,
            UserId = m.UserId,
            GymId = m.GymId,
            Status = (int) m.MembershipStatus,
            MembershipType = new MembershipTypesResponse
            {
                Id = m.MembershipTypeId,
                Duration = m.MembershipTypeDuration,
                Name = m.MembershipTypeName,
                Price = m.Price
            }
        }).ToList();

        var paginatedList = new PaginatedList<MembershipResponse>(
            membershipResponses,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(paginatedList);
    }
}