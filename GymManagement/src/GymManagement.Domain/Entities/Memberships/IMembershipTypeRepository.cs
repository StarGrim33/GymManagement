namespace GymManagement.Domain.Entities.Memberships;

public interface IMembershipTypeRepository
{
    Task<MembershipType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}