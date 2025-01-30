using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Application.Users.GetUser;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return new UserResponse();
        }

        var userResponse = new UserResponse
        {
            Id = user.Id,
            Address = user.Address,
            TrainingSessions = user.TrainingSessions,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive,
            Memberships = user.Memberships,
            Role = user.Role,
        };

        return Result.Success(userResponse);
    }
}