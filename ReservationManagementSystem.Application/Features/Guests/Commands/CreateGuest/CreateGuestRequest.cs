using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.CreateGuest;

public sealed record CreateGuestRequest(string Email, string FirstName, string LastName, string PhoneNumber) : IRequest<GuestResponse>;
