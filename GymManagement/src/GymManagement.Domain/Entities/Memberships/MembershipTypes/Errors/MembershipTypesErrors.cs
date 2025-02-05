using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Memberships.MembershipTypes.Errors;

public static class MembershipTypesErrors
{
    public static Error NotFound =
        new("MembershipType.NotFound",
            "MembershipType was not found");    
    
    public static Error Exists =
        new("MembershipType.AlreadyExists",
            "MembershipType was is already exists");

    public static Error EmptyName = new("MembershipType.EmptyName",
        "MembershipType request does not contain a name");
}