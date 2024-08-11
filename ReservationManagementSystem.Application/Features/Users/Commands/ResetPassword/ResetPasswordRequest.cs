using MediatR;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Users.Commands.ResetPassword;

public class ResetPasswordRequest : IRequest<Result<string>>
{
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required string Password { get; set; }
}
