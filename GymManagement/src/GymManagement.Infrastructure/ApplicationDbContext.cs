using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Exceptions;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Invoices;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Memberships.MembershipTypes;
using GymManagement.Domain.Entities.Trainers;
using GymManagement.Domain.Entities.Users;
using GymManagement.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GymManagement.Infrastructure;

public class ApplicationDbContext(
    DbContextOptions options,
    ILogger<ApplicationDbContext> logger)
    : DbContext(options), IUnitOfWork, IApplicationDbContext
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private IDbContextTransaction? _currentTransaction;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            AddDomainEventsAsOutboxMessages();

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred", ex);
        }
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No active transaction to commit.");
        }

        try
        {
            await SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        finally
        {
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No active transaction to rollback.");
        }

        try
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    private void AddDomainEventsAsOutboxMessages()
    {
        var outboxMessages = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.CreateVersion7(),
                DateTime.UtcNow,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)))
            .ToList();

        AddRange(outboxMessages);
    }

    // IApplicationDbContext implementation
    public async Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
    {
        await Set<T>().AddAsync(entity, cancellationToken);
    }

    public void Update<T>(T entity) where T : class
    {
        Set<T>().Update(entity);
    }

    public void Remove<T>(T entity) where T : class
    {
        Set<T>().Remove(entity);
    }

    public async Task<T?> FindAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : class
    {
        return await Set<T>().FindAsync([id], cancellationToken);
    }

    public async Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken = default) where T : class
    {
        return await Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Set<User>()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<Gym?> GetGymByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Set<Gym>()
            .FirstOrDefaultAsync(g => g.Name == name, cancellationToken);
    }

    public async Task<Gym?> GetGymWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Set<Gym>()
            .AsSplitQuery()
            .Include(g => g.GymAmenities)
            .Include(g => g.Trainers)
            .Include(g => g.Equipment)
            .Include(g => g.Memberships)
            .Include(g => g.TrainingSessions)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<List<User>> GetPagedUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await Set<User>()
            .OrderBy(u => u.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Gym> Gyms => Set<Gym>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<MembershipType> MembershipTypes => Set<MembershipType>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
}