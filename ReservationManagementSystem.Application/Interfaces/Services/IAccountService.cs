using ReservationManagementSystem.Application.DTOs.Account;
using ReservationManagementSystem.Application.Features.Users.Commands.AuthenticateUser;
using ReservationManagementSystem.Application.Features.Users.Commands.ConfirmEmail;
using ReservationManagementSystem.Application.Features.Users.Commands.ForgotPassword;
using ReservationManagementSystem.Application.Features.Users.Commands.RegisterUser;
using ReservationManagementSystem.Application.Features.Users.Commands.ResetPassword;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Interfaces.Services;

public interface IAccountService
{
    Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticateUserRequest request);
    Task<Result<string>> RegisterAsync(CreateUserRequest request);
    Task<Result<string>> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result<string>> ForgotPassword(ForgotPasswordRequest request);
    Task<Result<string>> ResetPassword(ResetPasswordRequest request);
}
