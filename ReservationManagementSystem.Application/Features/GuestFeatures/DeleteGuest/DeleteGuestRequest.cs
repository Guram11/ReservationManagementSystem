using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.DeleteGuest;

public sealed record DeleteGuestRequest(Guid Id) : IRequest<GuestResponse>;
