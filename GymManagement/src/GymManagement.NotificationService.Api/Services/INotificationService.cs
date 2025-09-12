namespace GymManagement.NotificationService.Api.Services;

public interface INotificationService
{
    Task SendNotificationAsync(Guid userId, string subject, string body);
}