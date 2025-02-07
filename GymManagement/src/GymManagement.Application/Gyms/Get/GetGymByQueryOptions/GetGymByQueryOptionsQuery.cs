using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Gyms.Get.GetGymByQueryOptions;

public record GetGymByQueryOptionsQuery(
    Guid GymId,
    bool DoIncludeAmenities,
    bool DoIncludeTrainers,
    bool DoIncludeEquipment,
    bool DoIncludeMemberships,
    bool DoIncludeTrainingSessions,
    bool IsAsNoTracking) : IQuery<GymResponse>;