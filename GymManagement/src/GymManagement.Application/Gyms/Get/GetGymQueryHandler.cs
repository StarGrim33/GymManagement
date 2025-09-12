using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Application.Gyms.Get;

internal sealed class GetGymQueryHandler(IGymRepository repository) : IQueryHandler<GetGymQuery, GymResponse>
{
    public async Task<Result<GymResponse>> Handle(GetGymQuery request, CancellationToken cancellationToken)
    {
        var gym = await repository.GetByIdAsync(request.GymId, cancellationToken);

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