using GymManagement.Application.Abstractions.Messaging;
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
            IncludeMemberships = request.DoIncludeMemberships,
            AsNoTracking = request.IsAsNoTracking,
            IncludeAmenities = request.DoIncludeAmenities,
            IncludeEquipment = request.DoIncludeEquipment,
            IncludeTrainers = request.DoIncludeTrainers,
            IncludeTrainingSessions = request.DoIncludeTrainingSessions
        };

        var gym = await repository.GetAsync(x => x.Id == request.GymId, queryOptions, cancellationToken);

        if (gym == null)
        {
            return new GymResponse();
        }

        var gymResponse = new GymResponse
        {
            Id = gym.Id,
            Name = gym.Name,
            Description = gym.Description,
            Address = gym.Address,
            Schedule = gym.Schedule,
            GymAmenities = gym.GymAmenities.ToList(),
            Trainers = gym.Trainers.ToList(),
            Equipment = gym.Equipment.ToList(),
            Memberships = gym.Memberships.ToList(),
            TrainingSessions = gym.TrainingSessions.ToList()
        };

        return Result.Success(gymResponse);
    }
}