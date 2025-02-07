using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Gyms.Errors;

namespace GymManagement.Application.Gyms.Update;

internal sealed class UpdateGymCommandHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateGymCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateGymCommand request, CancellationToken cancellationToken)
    {
        var gym = await gymRepository.GetByIdAsync(request.GymId, cancellationToken);

        if (gym is null)
        {
            return Result.Failure<Guid>(GymErrors.NotFound);
        }

        gym.Update(request.Name, request.Description, request.Address, request.Schedule);

        await gymRepository.Update(gym);

        return Result.Success(gym.Id);
    }
}