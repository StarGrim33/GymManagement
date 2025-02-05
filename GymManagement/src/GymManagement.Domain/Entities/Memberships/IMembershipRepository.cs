namespace GymManagement.Domain.Entities.Memberships;

public interface IMembershipRepository
{
    Task<Membership?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

    Task<List<Membership>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    void Add(Membership membership);
}