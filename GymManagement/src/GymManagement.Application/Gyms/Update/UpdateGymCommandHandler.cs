using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities;

namespace GymManagement.Application.Gyms.Update;

internal sealed class UpdateGymCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateGymCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateGymCommand request, CancellationToken cancellationToken)
    {
        var gym = await dbContext.GetGymWithDetailsAsync(request.GymId, cancellationToken);

        if (gym is null)
        {
            return Result.Failure<Guid>(new Error("Gym.NotFound", "Спортзал не найден"));
        }

        var addressResult = Address.Create(request.Street, request.City, request.ZipCode, request.Region, request.Country);

        if (addressResult.IsFailure)
            return Result.Failure<Guid>(addressResult.Error);

        var updateResult = gym.Update(
            request.Name,
            request.Description,
            addressResult.Value);

        if (updateResult.IsFailure)
        {
            return Result.Failure<Guid>(updateResult.Error);
        }

        dbContext.UpdateEntity(gym);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(gym.Id);
    }
}