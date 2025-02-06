using GymManagement.Application.Abstractions.Messaging;
using GymManagement.Application.Common;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Users;

namespace GymManagement.Application.Users.GetUser.GetAllUsers
{
    internal sealed class GetAllUsersHandler(IUserRepository repository)
        : IQueryHandler<GetAllUsersQuery, PaginatedList<UserResponse>>
    {
        public async Task<Result<PaginatedList<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var totalCount = await repository.GetTotalCountAsync(cancellationToken);

            var users = await repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            var userResponses = users.Select(u => new UserResponse
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                DateOfBirth = u.DateOfBirth,
                IsActive = u.IsActive,
                Role = u.Role,
                Address = u.Address
            }).ToList();

            var paginatedList = new PaginatedList<UserResponse>(
                userResponses,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result.Success(paginatedList);
        }
    }
}
