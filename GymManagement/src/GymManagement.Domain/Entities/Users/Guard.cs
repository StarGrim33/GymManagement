namespace GymManagement.Domain.Entities.Users;

public static class Guard
{
    public static void AgainstNull<T>(T value, string? message = null) where T : class
    {
        if (value == null)
            throw new ArgumentNullException(message ?? $"Argument {typeof(T).Name} cannot be null");
    }

    public static void AgainstNullOrEmpty(string value, string? message = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value), message ?? "Value cannot be null or empty");
    }
}