using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Api.Endpoints.Users;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber,
    DateTime DateOfBirth,
    bool IsActive,
    int RoleId,
    string Street,
    string City,
    string ZipCode);