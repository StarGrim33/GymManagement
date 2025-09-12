using GymManagement.Domain.Entities.Users.Events;
using GymManagement.NotificationService.Api.Messages;
using GymManagement.NotificationService.Api.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>(); // Регистрация обработчика событий

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://rabbitmq", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("user-created-queue", e =>
        {
            e.Bind<UserCreatedDomainEvent>(); // Явная привязка события к очереди
            e.ConfigureConsumer<UserCreatedConsumer>(context); // Подписка на очередь
        });
    });
});

builder.Services.AddLogging();

builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

await app.RunAsync();
