using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.UpdateGuest;

public sealed record UpdateGuestRequest(Guid Id, string Email, string FirstName, string LastName, string PhoneNumber) : IRequest<Result<GuestResponse>>;
