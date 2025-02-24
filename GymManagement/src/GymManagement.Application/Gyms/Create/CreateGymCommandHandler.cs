using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Gyms.Errors;

namespace GymManagement.Application.Gyms.Create;

internal sealed class CreateGymCommandHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateGymCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateGymCommand request, CancellationToken cancellationToken)
    {
        var gym = await gymRepository.GetByNameAsync(request.Name.Value, cancellationToken);

        if (gym is not null)
        {
            return Result.Failure<Guid>(GymErrors.Exists);
        }

        var newGym = Gym.Create(
            request.Name,
            request.Description,
            request.Address,
            request.Schedule);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(newGym.Id);
    }
}