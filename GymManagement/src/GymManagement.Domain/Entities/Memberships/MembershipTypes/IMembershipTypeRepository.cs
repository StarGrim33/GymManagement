namespace GymManagement.Domain.Entities.Memberships.MembershipTypes;

public interface IMembershipTypeRepository
{
    Task<MembershipType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(MembershipType membershipType);
}