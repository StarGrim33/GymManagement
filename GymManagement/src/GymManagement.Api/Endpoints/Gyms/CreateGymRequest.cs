using GymManagement.Domain.Entities;

namespace GymManagement.Api.Endpoints.Gyms;

public record CreateGymRequest(
    string Name,
    string Description,
    string Street,
    string City,
    string ZipCode,
    string Region,
    string Country);