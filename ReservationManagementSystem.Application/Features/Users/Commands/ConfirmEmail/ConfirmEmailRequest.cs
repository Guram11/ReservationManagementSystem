using MediatR;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Users.Commands.ConfirmEmail;

public class ConfirmEmailRequest : IRequest<Result<string>>
{
   public required string UserId { get; set; }
   public required string Code { get; set; }
}
