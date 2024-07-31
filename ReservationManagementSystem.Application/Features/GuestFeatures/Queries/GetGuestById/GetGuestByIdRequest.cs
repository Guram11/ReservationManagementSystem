using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.Queries.GetGuestById;

public sealed record GetGuestByIdRequest(Guid Id) : IRequest<GuestResponse>;
