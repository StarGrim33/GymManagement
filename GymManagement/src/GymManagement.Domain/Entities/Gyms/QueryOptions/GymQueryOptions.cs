namespace GymManagement.Domain.Entities.Gyms.QueryOptions;

public class GymQueryOptions
{
    public bool IncludeAmenities { get; init; }

    public bool IncludeTrainers { get; init; }

    public bool IncludeEquipment { get; init; }

    public bool IncludeMemberships { get; init; }

    public bool IncludeTrainingSessions { get; init; }

    public bool AsNoTracking { get; init; }
}