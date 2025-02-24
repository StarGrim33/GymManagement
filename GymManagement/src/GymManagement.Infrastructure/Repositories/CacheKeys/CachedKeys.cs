namespace GymManagement.Infrastructure.Repositories.CacheKeys;

public static class CachedKeys
{
    public static string UserById(Guid userId) => $"user-{userId}";

    public static string UserByEmail(string email) => $"user-{email}";

    public static string UsersPaged(int pageNumber, int pageSize) => $"users-page-{pageNumber}-size-{pageSize}";

    public static string UsersTotalCount => "users-total-count";

    public static string GymByName(string? name) => $"gym-{name}";

    public static string GymById(Guid gymId) => $"gym-{gymId}";

    public static string GymsPaged(int pageNumber, int pageSize) => $"gyms-page-{pageNumber}-size-{pageSize}";

    public static string GymsTotalCount => "gyms-total-count";

    public static string MembershipById(Guid membershipId) => $"membership-{membershipId}";

    public static string MembershipsPaged(int pageNumber, int pageSize) => $"memberships-page-{pageNumber}-size-{pageSize}";

    public static string MembershipsTotalCount => "memberships-total-count";

    public static string MembershipTypeById(Guid id) => $"membershipType-{id}";

    public static string MembershipTypeByName(string name) => $"membershipType-{name}";

    public static string InvoiceById(Guid id) => $"invoice-{id}";

    public static string TrainerById(Guid id) => $"trainer-{id}";

    public static string TrainerByName(string name) => $"trainer-{name}";

    public static string TrainerByEmail(string email) => $"trainer-email-{email}";
}