using MediatR;
using ReservationManagementSystem.Application.DTOs.Account;
using ReservationManagementSystem.Application.Wrappers;
using System.Text.Json.Serialization;

namespace ReservationManagementSystem.Application.Features.Users.Commands.AuthenticateUser;

public class AuthenticateUserRequest : IRequest<Result<AuthenticationResponse>>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    [JsonIgnore]
    public string? IpAddress { get; set; }
}
