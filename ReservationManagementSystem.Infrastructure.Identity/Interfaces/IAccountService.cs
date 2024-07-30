using ReservationManagementSystem.Infrastructure.Identity.Models.Account;
using ReservationManagementSystem.Infrastructure.Identity.Wrappers;

namespace ReservationManagementSystem.Infrastructure.Identity.Interfaces;

public interface IAccountService
{
    Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
    Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
    Task<Response<string>> ConfirmEmailAsync(string userId, string code);
    Task ForgotPassword(ForgotPasswordRequest model, string origin);
    Task<Response<string>> ResetPassword(ResetPasswordRequest model);
}
