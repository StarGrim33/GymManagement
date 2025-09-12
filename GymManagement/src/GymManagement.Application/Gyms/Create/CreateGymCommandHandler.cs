using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.OperatingHoursModel;

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

        var addressResult = Address.Create(request.Street, request.City, request.ZipCode, request.Region, request.Country);
        if (addressResult.IsFailure)
            return Result.Failure<Guid>(addressResult.Error);

        var defaultRange = TimeRange.Create(new TimeOnly(6, 0), new TimeOnly(23, 0)).Value;
        var days = Enum.GetValues<DayOfWeek>()
            .Select(d => DaySchedule.Create(d, new[] { defaultRange }).Value);
        var operatingHours = OperatingHours.Create(days).Value;

        var gymResult = Gym.Create(
            request.Name,
            request.Description,
            addressResult.Value,
            operatingHours);

        if (gymResult.IsFailure)
        {
            return Result.Failure<Guid>(gymResult.Error);
        }

        await dbContext.AddEntityAsync(gymResult.Value, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(gymResult.Value.Id);
    }
}