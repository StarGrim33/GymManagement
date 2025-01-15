using GymManagement.Application.Abstractions.Email;

namespace GymManagement.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Domain.Entities.Email recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}