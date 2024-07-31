using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.Commands.DeleteGuest;

public sealed record DeleteGuestRequest(Guid Id) : IRequest<GuestResponse>;
