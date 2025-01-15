using GymManagement.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

}