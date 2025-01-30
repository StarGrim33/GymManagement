using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities;

namespace GymManagement.Api.Controllers.Gyms;

public record CreateGymRequest(Name Name,
    Description Description,
    Address Address,
    Schedule Schedule);