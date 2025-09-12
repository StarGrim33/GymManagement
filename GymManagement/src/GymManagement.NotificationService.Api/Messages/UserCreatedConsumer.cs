using GymManagement.Domain.Entities.Users.Events;
using GymManagement.NotificationService.Api.Services;
using MassTransit;

namespace GymManagement.NotificationService.Api.Messages;

public sealed class UserCreatedConsumer(INotificationService notificationService) 
    : IConsumer<UserCreatedDomainEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedDomainEvent> context)
    {
        var @event = context.Message;

        await notificationService.SendNotificationAsync(@event.UserId, "Welcome!", "Thank you for registering!");
    }
}