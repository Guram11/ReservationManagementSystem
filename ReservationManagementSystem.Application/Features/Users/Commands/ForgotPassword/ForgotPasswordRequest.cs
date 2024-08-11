using MediatR;
using ReservationManagementSystem.Application.Wrappers;
using System.Text.Json.Serialization;

namespace ReservationManagementSystem.Application.Features.Users.Commands.ForgotPassword;

public class ForgotPasswordRequest : IRequest<Result<string>>
{
    public required string Email { get; set; }
    [JsonIgnore]
    public string? Origin { get; set; }
}
