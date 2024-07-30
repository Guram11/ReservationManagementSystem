using ReservationManagementSystem.Infrastructure.Identity.Models.Email;

namespace ReservationManagementSystem.Infrastructure.Identity.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
}
