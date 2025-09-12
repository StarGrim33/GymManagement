using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Gyms;

namespace GymManagement.Api.Endpoints.Gyms;

public record UpdateGymRequest(
    Guid GymId,
    string Name,
    string Description,
    string Street,
    string City,
    string ZipCode,
    string Schedule);