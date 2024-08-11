using MimeKit;

namespace ReservationManagementSystem.Application.Interfaces.Services;

public interface IEmailSender
{
    Task SendAsync(MimeMessage message);
}
