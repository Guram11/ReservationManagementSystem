using MediatR;
using ReservationManagementSystem.Application.DTOs.Account;

namespace ReservationManagementSystem.Application.Features.Users.Commands.AuthenticateUser;

public sealed record AuthenticateUserRequest (string Email, string Password, string IpAddress) : IRequest<AuthenticationResponse>;
