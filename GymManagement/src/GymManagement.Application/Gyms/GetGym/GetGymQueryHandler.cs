using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Application.Gyms.GetGym;

internal sealed class GetGymQueryHandler : IQueryHandler<GetGymQuery, GymResponse>
{
    private readonly IGymRepository _repository;

    public GetGymQueryHandler(IGymRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GymResponse>> Handle(GetGymQuery request, CancellationToken cancellationToken)
    {
        var gym = await _repository.GetByIdAsync(request.GymId, cancellationToken);

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