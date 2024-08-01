using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.UpdateGuest;

public sealed record UpdateGuestRequest(Guid Id, string Email, string FirstName, string LastName, string PhoneNumber) : IRequest<GuestResponse>;
