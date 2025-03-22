namespace GymManagement.Api.Endpoints.Gyms;

public record GetGymByQueryOptionsRequest(
    Guid GymId,
    bool DoIncludeAmenities,
    bool DoIncludeTrainers,
    bool DoIncludeEquipment,
    bool DoIncludeMemberships,
    bool DoIncludeTrainingSessions,
    bool IsAsNoTracking);