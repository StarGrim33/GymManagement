namespace GymManagement.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(string recipient, string subject, string body);

    //ToDo: Сделать реальный email сервис
}