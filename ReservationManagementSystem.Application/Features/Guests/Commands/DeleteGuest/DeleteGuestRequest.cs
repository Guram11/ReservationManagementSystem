using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.DeleteGuest;

public sealed record DeleteGuestRequest(Guid Id) : IRequest<GuestResponse>;
