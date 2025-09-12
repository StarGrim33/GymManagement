namespace GymManagement.Api.Endpoints.Gyms;

public record UpdateGymRequest(
    string Name,
    string Description,
    string Street,
    string City,
    string ZipCode,
    string Region,
    string Country);