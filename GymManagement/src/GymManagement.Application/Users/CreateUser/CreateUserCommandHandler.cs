using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Users;
using GymManagement.Domain.Entities.Users.Errors;

namespace GymManagement.Application.Users.CreateUser;

internal sealed class CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = NormalizeEmail(request.Email.Value);
        var existingUser = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (existingUser != null)
        {
            return Result.Failure<Guid>(UserErrors.Found);
        }

        var newUser = User.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.DateOfBirth,
            request.IsActive,
            request.Role,
            request.Address
        );

        await userRepository.AddAsync(newUser, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(newUser.Id);
    }

    private static string NormalizeEmail(string emailValue)
    {
        var parts = emailValue.Split('@');

        if (parts.Length != 2)
            return emailValue;

        var name = parts[0].Replace(".", "");

        if (name.IndexOf('+') > 0)
            name = name[..name.IndexOf('+')];

        return name + '@' + parts[1];
    }
}