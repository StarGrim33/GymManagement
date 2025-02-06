using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Application.Common;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Memberships.Errors;

namespace GymManagement.Application.Gyms.GetGym.GetAll;

internal sealed class GetAllGymsHandler(IGymRepository repository)
    : IQueryHandler<GetAllGymsQuery, PaginatedList<GymResponse>>
{
    public async Task<Result<PaginatedList<GymResponse>>> Handle(GetAllGymsQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await repository.GetTotalCountAsync(cancellationToken);

        var gyms = await repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        if (gyms.Count == 0)
            return Result.Failure<PaginatedList<GymResponse>>(MembershipErrors.NotFound);

        var gymResponses = gyms.Select(g => new GymResponse
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description,
            Address = g.Address,
            Schedule = g.Schedule
        }).ToList();

        var paginatedList = new PaginatedList<GymResponse>(
            gymResponses,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(paginatedList);
    }
}