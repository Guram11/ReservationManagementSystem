using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.DeleteGuest;

public sealed record DeleteGuestRequest(Guid Id) : IRequest<Result<GuestResponse>>;
