using GymManagement.Domain.Entities;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Api.Endpoints.Users;

public record CreateUserRequest(FirstName FirstName,
    LastName LastName,
    Email Email,
    string Password,
    string PhoneNumber,
    DateTime DateOfBirth,
    bool IsActive,
    int RoleId,
    Address Address);