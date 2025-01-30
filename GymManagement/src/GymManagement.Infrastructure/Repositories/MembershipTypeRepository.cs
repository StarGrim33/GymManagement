using GymManagement.Domain.Entities.Memberships.MembershipTypes;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class MembershipTypeRepository(ApplicationDbContext dbContext) 
    : Repository<MembershipType>(dbContext), IMembershipTypeRepository
{
    
}