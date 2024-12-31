namespace GymManagement.Domain.Entities.Memberships;

public interface IMembershipRepository
{
    Task<Membership> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}