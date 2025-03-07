﻿using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Application.Gyms.Get;

internal sealed class GetGymQueryHandler(IGymRepository repository) : IQueryHandler<GetGymQuery, GymResponse>
{
    public async Task<Result<GymResponse>> Handle(GetGymQuery request, CancellationToken cancellationToken)
    {
        var gym = await repository.GetByIdAsync(request.GymId, cancellationToken);

        if (gym == null)
        {
            return new GymResponse();
        }

        var gymResponse = new GymResponse
        {
            Id = gym.Id,
            Name = new Name(gym.Name),
            Description = new Description(gym.Description),
            Address = new Address(gym.Address.Street, gym.Address.City, gym.Address.ZipCode),
            Schedule = new Schedule(gym.Schedule),

            GymAmenities = gym.GymAmenities?.ToList() ?? [],
            Trainers = gym.Trainers?.ToList() ?? [],
            Equipment = gym.Equipment?.ToList() ?? [],
            Memberships = gym.Memberships?.ToList() ?? [],
            TrainingSessions = gym.TrainingSessions?.ToList() ?? []
        };

        return Result.Success(gymResponse);
    }
}