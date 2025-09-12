namespace GymManagement.NotificationService.Api.Services;

public class NotificationService(ILogger<NotificationService> logger) : INotificationService
{
    public async Task SendNotificationAsync(Guid userId, string subject, string body)
    {
        // Логика отправки уведомлений (например, через SMTP или внешний API)
        logger.LogInformation($"Sending notification to user {userId}: {subject} - {body}");
    }
}