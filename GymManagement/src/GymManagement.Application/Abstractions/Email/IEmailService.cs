namespace GymManagement.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(Domain.Entities.Email recipient, string subject, string body);

    //ToDo: Сделать реальный email сервис
}