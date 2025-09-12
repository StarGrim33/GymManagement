using GymManagement.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure.DomainEvents;

internal sealed class DomainEventsDispatcher(IServiceProvider serviceProvider) 
    : IDomainEventsDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());

            var handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                if (handler is null)
                    continue;

                var handlerMethod = handlerType.GetMethod("Handle");

                if (handlerMethod is not null)
                {
                    await (Task)handlerMethod.Invoke(handler, [domainEvents, cancellationToken])!;
                }
            }
        }
    }
}

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T domainEvent, CancellationToken cancellationToken);
}