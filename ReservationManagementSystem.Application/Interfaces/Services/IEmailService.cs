using ReservationManagementSystem.Application.DTOs.Email;

namespace ReservationManagementSystem.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
}
