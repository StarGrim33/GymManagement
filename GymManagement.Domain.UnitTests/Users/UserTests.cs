using FluentAssertions;
using GymManagement.Domain.Entities.Users;
using GymManagement.Domain.Entities.Users.Events;

namespace GymManagement.Domain.UnitTests.Users;

public class UserTests : BaseTest
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        // Act
        var user = UserData.CreateTestUser();

        // Assert
        user.FirstName.Should().Be(UserData.FirstName);
        user.LastName.Should().Be(UserData.LastName);
        user.Email.Should().Be(UserData.Email);
        user.PhoneNumber.Should().Be(UserData.PhoneNumber);
        user.DateOfBirth.Should().Be(UserData.DateOfBirth);
        user.IsActive.Should().Be(UserData.IsActive);
        user.Role.Should().Be(UserData.Role);
        user.Street.Should().Be(UserData.Street);
        user.City.Should().Be(UserData.City);
        user.ZipCode.Should().Be(UserData.ZipCode);
    }

    [Fact]
    public void Create_Should_ThrowException_When_FirstNameIsNull()
    {
        // Act

        Action act = () => User.Create(
            "",
            UserData.LastName,
            UserData.Email,
            UserData.PhoneNumber,
            UserData.DateOfBirth,
            UserData.IsActive,
            UserData.Role,
            UserData.Street,
            UserData.City,
            UserData.ZipCode);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Create_Should_RaiseUserCreatedDomainEvent()
    {
        // Act
        var user = UserData.CreateTestUser();

        // Assert
        var userCreatedDomainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);

        userCreatedDomainEvent.UserId.Should().Be(user.Id);
    }
}