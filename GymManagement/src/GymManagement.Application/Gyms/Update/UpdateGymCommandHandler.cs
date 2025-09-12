using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;

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

        var updateResult = gym.Update(
            request.Name, 
            request.Description, 
            request.Street, 
            request.City, 
            request.ZipCode, 
            request.Schedule);

        if (updateResult.IsFailure)
        {
            return Result.Failure<Guid>(updateResult.Error);
        }

        dbContext.Update(gym);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(gym.Id);
    }
}