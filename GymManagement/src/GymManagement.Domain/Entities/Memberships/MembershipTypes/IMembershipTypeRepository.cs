namespace GymManagement.Domain.Entities.Memberships.MembershipTypes;

public interface IMembershipTypeRepository
{
    Task<MembershipType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MembershipType?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    void Add(MembershipType membershipType);
}