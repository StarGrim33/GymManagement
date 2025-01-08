using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Users.Errors;

public static class UserErrors
{
    public static Error NotFound =
        new("User.Found",
            "User with the specified identifier was not found");

    public static Error NotUnFrozen =
        new("User.NotUnFrozen",
            "The current user is not frozen");
}