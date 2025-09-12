using GymManagement.Domain.Entities.Users;

namespace GymManagement.Domain.UnitTests.Users;

public static class UserData
{
    public static readonly string FirstName = "First";
    public static readonly string LastName = "Last";
    public static readonly string Email = "test@test.com";
    public static readonly string PhoneNumber = "1234567890";
    public static readonly DateTime DateOfBirth = new(1990, 1, 1);
    public static readonly bool IsActive = true;
    public static readonly Role Role = Role.Registered;
    public static readonly string Street = "123 Main St";
    public static readonly string City = "Anytown";
    public static readonly string ZipCode = "60031";

    public static User CreateTestUser()
    {
        return User.Create(
            FirstName,
            LastName,
            Email,
            PhoneNumber,
            DateOfBirth,
            IsActive,
            Role,
            Street,
            City,
            ZipCode);
    }
}