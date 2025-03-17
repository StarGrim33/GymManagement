using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.Memberships.Errors;

public static class MembershipErrors
{
    public static Error NotFound =
        new("Membership.Found",
            "Membership was not found");

    public static Error NotPaid =
        new("Membership.NotPaid",
            "The current membership is awaiting payment");

    public static Error NotActivated =
        new("Membership.NotActivated",
            "The current membership is not awaiting payment");

    public static Error NotUnFrozen =
        new("Membership.NotUnFrozen",
            "The current membership is not frozen");

    public static Error Overlap =
        new("Membership.Overlap",
            "The current membership is being overlapped by another administrator");

    public static Error Unauthorized =
        new("Membership.Unauthorized",
            "The current membership is not authorized to perform this action");
}