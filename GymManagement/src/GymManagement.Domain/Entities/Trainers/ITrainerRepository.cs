namespace GymManagement.Domain.Entities.Trainers;

public interface ITrainerRepository
{
    Task<Trainer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Trainer>?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<Trainer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task AddAsync(Trainer trainer, CancellationToken cancellationToken = default);

    Task Update(Trainer trainer);

    Task Delete(Trainer trainer);
}