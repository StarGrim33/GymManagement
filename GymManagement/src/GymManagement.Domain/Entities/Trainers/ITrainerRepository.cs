namespace GymManagement.Domain.Entities.Trainers;

public interface ITrainerRepository
{
    Task<Trainer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}