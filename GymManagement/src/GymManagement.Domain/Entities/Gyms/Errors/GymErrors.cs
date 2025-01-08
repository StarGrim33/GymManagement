using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Gyms.Errors;

public static class GymErrors
{
    public static Error NotFound =
        new("Gym.Found",
            "Membership was not found");
}