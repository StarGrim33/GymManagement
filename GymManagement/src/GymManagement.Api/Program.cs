using Asp.Versioning;
using GymManagement.Api.Endpoints.Gyms;
using GymManagement.Api.Endpoints.Memberships;
using GymManagement.Api.Endpoints.MembershipTypes;
using GymManagement.Api.Endpoints.Users;
using GymManagement.Api.Extensions;
using GymManagement.Api.OpenApi;
using GymManagement.Application;
using GymManagement.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace GymManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    var descriptions = app.DescribeApiVersions();

                    if (!descriptions.Any())
                    {
                        throw new InvalidOperationException("No API versions found. Check your API versioning configuration.");
                    }

                    foreach (var description in descriptions)
                    {
                        var url = $"{description.GroupName}/swagger.json";
                        var name = description.GroupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }
                });

                app.ApplyMigrations();
                //app.SeedData();
            }

            app.UseHttpsRedirection();

            app.UseRequestContextMiddleware();

            app.UseSerilogRequestLogging();

            app.UseCustomExceptionHandler();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            var apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

            var routeGroupBuilder = app
                .MapGroup("api/v{version:apiVersion}")
                .WithApiVersionSet(apiVersionSet);

            routeGroupBuilder.MapUserEndpoints();
            routeGroupBuilder.MapGymEndpoints();
            routeGroupBuilder.MapMembershipEndpoints();
            routeGroupBuilder.MapMembershipTypesEndpoints();

            app.MapHealthChecks("health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.Run();
        }
    }
}
