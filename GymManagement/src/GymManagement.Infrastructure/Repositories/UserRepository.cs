using GymManagement.Domain.Entities.Users;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationDbContext dbContext) 
    : Repository<User>(dbContext), IUserRepository
{
    
}