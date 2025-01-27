using GymManagement.Domain.Entities.Memberships;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class MembershipRepository(ApplicationDbContext dbContext) 
    : Repository<Membership>(dbContext), IMembershipRepository
{
    
}