using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Application.Gyms.Get;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Gyms.QueryOptions;

namespace GymManagement.Application.Gyms.Get.GetGymByQueryOptions;

internal sealed class GetGymByQueryOptionsHandler(IGymRepository repository)
    : IQueryHandler<GetGymByQueryOptionsQuery, GymResponse>
{
    public async Task<Result<GymResponse>> Handle(GetGymByQueryOptionsQuery request, CancellationToken cancellationToken)
    {
        var queryOptions = new GymQueryOptions
        {
            AsNoTracking = request.IsAsNoTracking,
        };

        var gym = await repository.GetAsync(x => x.Id == request.GymId, queryOptions, cancellationToken);

        if (gym == null)
        {
            return Result.Failure<GymResponse>(new Error("Gym.NotFound", "Спортзал не найден"));
        }

        var gymResponse = new GymResponse
        {
            Id = gym.Id,
            Name = gym.Name ?? string.Empty,
            Description = gym.Description ?? string.Empty,
            Street = gym.Street ?? string.Empty,
            City = gym.City ?? string.Empty,
            ZipCode = gym.ZipCode ?? string.Empty,
            Schedule = gym.Schedule ?? string.Empty,
            GymAmenities = gym.GymAmenities?.ToList() ?? [],
            Trainers = gym.Trainers?.ToList() ?? [],
            Equipment = gym.Equipment?.ToList() ?? [],
            Memberships = gym.Memberships?.ToList() ?? [],
            TrainingSessions = gym.TrainingSessions?.ToList() ?? []
        };

        return Result.Success(gymResponse);
    }
}