namespace GymManagement.Domain.Entities.Memberships;

public interface IMembershipRepository
{
    Task<MembershipDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

    Task<List<MembershipDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    Task AddAsync(Membership membership, CancellationToken cancellationToken = default);

    Task Update(Membership membership);

    Task Delete(Membership membership);
}