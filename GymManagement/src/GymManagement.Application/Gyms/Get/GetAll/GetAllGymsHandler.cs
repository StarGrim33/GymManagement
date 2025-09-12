using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Application.Common;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Application.Gyms.Get.GetAll;

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
            return Result.Failure<PaginatedList<GymResponse>>(new Error("Gym.NotFound", "Спортзалы не найдены"));

        var gymResponses = gyms.Select(g => new GymResponse
        {
            Id = g.Id,
            Name = g.Name ?? string.Empty,
            Description = g.Description ?? string.Empty,
            Street = g.Street ?? string.Empty,
            City = g.City ?? string.Empty,
            ZipCode = g.ZipCode ?? string.Empty,
            Schedule = g.Schedule ?? string.Empty
        }).ToList();

        var paginatedList = new PaginatedList<GymResponse>(
            gymResponses,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(paginatedList);
    }
}