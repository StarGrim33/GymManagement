using GymManagement.Domain.Entities.Trainers;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class TrainerRepository(ApplicationDbContext dbContext) 
    : Repository<Trainer>(dbContext), ITrainerRepository
{
    public async Task<List<Trainer>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Trainer>()
            .Where(t => t.FirstName.Value + " " + t.LastName.Value == name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Trainer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Trainer>()
            .FirstOrDefaultAsync(t => t.Email.Value == email, cancellationToken);
    }
}