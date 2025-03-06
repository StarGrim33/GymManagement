using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Users.Errors;

public static class UserErrors
{
    public static readonly Error NotFound =
        new("User.NotFound",
            "User with the specified identifier was not found");

    public static readonly Error Found =
        new("User.Found",
            "User is already exists");

    public static readonly Error NotUnFrozen =
        new("User.NotUnFrozen",
            "The current user is not frozen");

    public static readonly Error InternalServerError =
        new("User.InternalServerError",
            "Internal server error occured");

    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials",
            "Invalid user credentials");
}