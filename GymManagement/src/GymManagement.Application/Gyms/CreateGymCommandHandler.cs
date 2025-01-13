using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Gyms.Errors;

namespace GymManagement.Application.Gyms;

internal sealed class CreateGymCommandHandler : ICommandHandler<CreateGymCommand, Guid>
{
    private readonly IGymRepository _gymRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGymCommandHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork)
    {
        _gymRepository = gymRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateGymCommand request, CancellationToken cancellationToken)
    {
        var gym = await _gymRepository.GetByNameAsync(request.Name.Value, cancellationToken);

        if (gym is not null)
        {
            return Result.Failure<Guid>(GymErrors.Exists);
        }

        var newGym = Gym.Create(
            request.Name,
            request.Description,
            request.Address,
            request.Schedule);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(newGym.Id);
    }
}