using GymManagement.Domain.Entities.Trainers;

namespace GymManagement.Infrastructure.Repositories;

internal sealed class TrainerRepository(ApplicationDbContext dbContext) 
    : Repository<Trainer>(dbContext), ITrainerRepository
{
    
}