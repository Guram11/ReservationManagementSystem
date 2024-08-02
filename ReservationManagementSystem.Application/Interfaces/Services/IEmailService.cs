using ReservationManagementSystem.Application.DTOs.Email;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Interfaces.Services;

public interface IEmailService
{
    Task<Result<string>> SendAsync(EmailRequest request);
}
