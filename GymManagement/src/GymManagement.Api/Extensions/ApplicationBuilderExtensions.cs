using GymManagement.Api.Middleware;
using GymManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        try
        {
            using var scope = app.ApplicationServices.CreateScope();

            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}