using GymManagement.Application.Abstractions.Authentication;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Users;
using GymManagement.Domain.Entities.Users.Errors;
using System.Security.Cryptography;

namespace GymManagement.Application.Users.CreateUser;

internal sealed class CreateUserCommandHandler(
    IApplicationDbContext dbContext,
    IAuthenticationService authenticationService)
    : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, 
        CancellationToken cancellationToken)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var existingUser = await dbContext.GetUserByEmailAsync(normalizedEmail, cancellationToken);

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
            request.Street,
            request.City,
            request.ZipCode
        );

        newUser.SetPassword(request.Password);

        try
        {
            var identityId = await authenticationService.RegisterAsync(
                newUser,
                request.Password,
                cancellationToken);

            newUser.SetIdentityId(identityId);

            await dbContext.AddAsync(newUser, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(newUser.Id);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(UserErrors.InternalServerError, ex.StackTrace ?? "", ex.Message);
        }
    }

    private static string NormalizeEmail(string emailValue)
    {
        var match = System.Text.RegularExpressions.Regex.Match(emailValue, "^(?<name>[^@]+)@(?<domain>.+)$");
        
        if (!match.Success)
            return emailValue;

        var name = match.Groups["name"].Value.Replace(".", "");
        var plusIndex = name.IndexOf('-');

        if (plusIndex > 0)
            name = name[..plusIndex];

        return $"{name}@{match.Groups["domain"].Value}";
    }

    private static string GenerateSecurePassword(int length = 12)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+";
        var random = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(random);

        var chars = new char[length];

        for (var i = 0; i < length; i++)
        {
            chars[i] = validChars[random[i] % validChars.Length];
        }

        return new string(chars);
    }
}