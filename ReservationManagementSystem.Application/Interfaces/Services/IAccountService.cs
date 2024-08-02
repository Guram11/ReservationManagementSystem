using ReservationManagementSystem.Application.DTOs.Account;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Interfaces.Services;

public interface IAccountService
{
    Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
    Task<Result<string>> RegisterAsync(RegisterRequest request, string origin);
    Task<Result<string>> ConfirmEmailAsync(string userId, string code);
    Task<Result<string>> ForgotPassword(ForgotPasswordRequest model, string origin);
    Task<Result<string>> ResetPassword(ResetPasswordRequest model);
}
