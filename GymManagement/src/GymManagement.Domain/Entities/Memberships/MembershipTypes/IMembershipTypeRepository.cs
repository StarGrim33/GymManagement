namespace GymManagement.Domain.Entities.Memberships.MembershipTypes;

public interface IMembershipTypeRepository
{
    Task<MembershipType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MembershipType?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task AddAsync(MembershipType membershipType, CancellationToken cancellationToken = default);

    Task Update(MembershipType membershipType);

    Task Delete(MembershipType membershipType);
}