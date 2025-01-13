using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Gyms.Errors;

public static class GymErrors
{
    public static Error NotFound =
        new("Gym.Found",
            "Gym was not found");    
    
    public static Error Exists =
        new("Gym.Exists",
            "Gym with the same name is already exists");
}