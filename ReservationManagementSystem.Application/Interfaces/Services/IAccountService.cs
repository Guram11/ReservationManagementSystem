using ReservationManagementSystem.Application.DTOs.Account;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Interfaces.Services;

public interface IAccountService
{
    Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
    Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
    Task<Response<string>> ConfirmEmailAsync(string userId, string code);
    Task ForgotPassword(ForgotPasswordRequest model, string origin);
    Task<Response<string>> ResetPassword(ResetPasswordRequest model);
}
