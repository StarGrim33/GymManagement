using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Application.Common;
using GymManagement.Application.MembershipTypes;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Memberships.Errors;
using GymManagement.Domain.Entities.Memberships.MembershipTypes;

namespace GymManagement.Application.Memberships.GetMembership.GetAllMemberships;

internal sealed class GetAllMembershipsHandler : IQueryHandler<GetAllMembershipsQuery, PaginatedList<MembershipResponse>>
{
    private readonly IMembershipRepository _repository;

    public GetAllMembershipsHandler(IMembershipRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PaginatedList<MembershipResponse>>> Handle(GetAllMembershipsQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _repository.GetTotalCountAsync(cancellationToken);

        var memberships = await _repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        if (memberships.Count == 0)
            return Result.Failure<PaginatedList<MembershipResponse>>(MembershipErrors.NotFound);

        var membershipResponses = memberships.Select(m => new MembershipResponse
        {
            Id = m.Id,
            Name = m.MembershipType.Name, 
            PriceAmount = m.PriceAmount,
            StartDate = m.StartDate,
            EndDate = m.EndDate,
            IsActive = m.IsActive,
            UserId = m.UserId,
            GymId = m.GymId,
            Status = (int) m.MembershipStatus,
            MembershipType = new MembershipTypesResponse
            {
                Id = m.MembershipTypeId,
                Duration = m.MembershipType.Duration,
                Name = m.MembershipType.Name,
                Price = m.MembershipType.Price
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