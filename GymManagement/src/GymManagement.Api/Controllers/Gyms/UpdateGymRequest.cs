using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Api.Controllers.Gyms;

public record UpdateGymRequest(Guid GymId,
    Name Name,
    Description Description,
    Address Address,
    Schedule Schedule);