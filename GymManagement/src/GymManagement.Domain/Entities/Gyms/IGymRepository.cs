namespace GymManagement.Domain.Entities.Gyms;

public interface IGymRepository
{
    Task<Gym> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}