using GymManagement.Application.Abstractions.Messaging;

namespace GymManagement.Application.Gyms.Get.GetGymByQueryOptions;

public record GetGymByQueryOptionsQuery(Guid GymId,
    bool IsAsNoTracking) : IQuery<GymResponse>;