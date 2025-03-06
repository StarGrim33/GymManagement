using Dapper;
using GymManagement.Application.Abstractions.Authentication;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Application.Abstractions.Email;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Gyms;
using GymManagement.Domain.Entities.Invoices;
using GymManagement.Domain.Entities.Memberships;
using GymManagement.Domain.Entities.Memberships.MembershipTypes;
using GymManagement.Domain.Entities.Trainers;
using GymManagement.Domain.Entities.Users;
using GymManagement.Infrastructure.Authentication;
using GymManagement.Infrastructure.Data;
using GymManagement.Infrastructure.Email;
using GymManagement.Infrastructure.Repositories;
using GymManagement.Infrastructure.Repositories.CachedRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GymManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<AdminAuthorizationDelegatingHandler>();

        AddPersistence(services, configuration);

        AddAuthentication(services, configuration);

        return services;
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

        services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
            {
                var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
                               throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine);
        });

        services.AddScoped<IUserRepository, UserRepository>();

        //services.Decorate<IUserRepository, CachedUserRepository>();

        services.AddScoped<IMembershipRepository, MembershipRepository>();

        //services.Decorate<IMembershipRepository, CachedMembershipRepository>();

        services.AddScoped<IMembershipTypeRepository, MembershipTypeRepository>();

        //services.Decorate<IMembershipTypeRepository, CachedMembershipTypeRepository>();

        services.AddScoped<IInvoiceRepository, InvoiceRepository>();

        //services.Decorate<IInvoiceRepository, CachedInvoiceRepository>();

        services.AddScoped<IGymRepository, GymRepository>();

        //services.Decorate<IGymRepository, CachedGymRepository>();

        services.AddScoped<ITrainerRepository, TrainerRepository>();

        //services.Decorate<ITrainerRepository, CachedTrainerRepository>();

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }
}