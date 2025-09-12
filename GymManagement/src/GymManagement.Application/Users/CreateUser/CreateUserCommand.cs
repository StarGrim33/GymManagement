using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Application.Users.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber,
    DateTime DateOfBirth,
    bool IsActive,
    Role Role,
    string Street,
    string City,
    string ZipCode) : ICommand<Guid>;