using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Application.Gyms.Create;

internal sealed class CreateGymCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateGymCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateGymCommand request, CancellationToken cancellationToken)
    {
        var existingGym = await dbContext.GetGymByNameAsync(request.Name, cancellationToken);

        if (existingGym is not null)
        {
            return Result.Failure<Guid>(new Error("Gym.AlreadyExists", "Спортзал с таким названием уже существует"));
        }

        var gymResult = Gym.Create(
            request.Name,
            request.Description,
            request.Street,
            request.City,
            request.ZipCode,
            request.Schedule);

        if (gymResult.IsFailure)
        {
            return Result.Failure<Guid>(gymResult.Error);
        }

        await dbContext.AddAsync(gymResult.Value, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(gymResult.Value.Id);
    }
}