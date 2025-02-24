using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Gyms.Errors;

namespace GymManagement.Application.Gyms.Delete;

internal sealed class DeleteGymCommandHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteGymCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteGymCommand request, CancellationToken cancellationToken)
    {
        var gym = await gymRepository.GetEntityByIdAsync(request.GymId, cancellationToken);

        if (gym is null)
        {
            return Result.Failure<Guid>(GymErrors.NotFound);
        }

        await gymRepository.Delete(gym);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(request.GymId);
    }
}