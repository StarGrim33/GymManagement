using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities;

namespace GymManagement.Api.Endpoints.Gyms;

public record CreateGymRequest(Name Name,
    Description Description,
    Address Address,
    Schedule Schedule);