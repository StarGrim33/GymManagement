namespace GymManagement.Domain.Entities.Users;

public sealed class Permission(int id, string name)
{
    public static readonly Permission UsersRead = new(1, "users:read");

    public int Id { get; init; } = id;

    public string Name { get; init; } = name;
}