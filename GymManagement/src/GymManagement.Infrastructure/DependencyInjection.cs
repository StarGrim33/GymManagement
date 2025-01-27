using Dapper;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Email;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Invoices;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Memberships.MembershipTypes;
using GymManagement.Domain.Entities.Trainers;
using GymManagement.Domain.Entities.Users;
using GymManagement.Infrastructure.Data;
using GymManagement.Infrastructure.Email;
using GymManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IEmailService, EmailService>();

        var connectionString = configuration.GetConnectionString("Database") ??
                               throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IMembershipRepository, MembershipRepository>();

        services.AddScoped<IMembershipTypeRepository, MembershipTypeRepository>();

        services.AddScoped<IInvoiceRepository, InvoiceRepository>();

        services.AddScoped<IGymRepository, GymRepository>();

        services.AddScoped<ITrainerRepository, TrainerRepository>();

        services.AddScoped<IUnitOfWork, ApplicationDbContext>();

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        return services;
    }
}