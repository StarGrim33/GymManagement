using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Api.Endpoints.Gyms;

public record UpdateGymRequest(Guid GymId,
    Name Name,
    Description Description,
    Address Address,
    Schedule Schedule);