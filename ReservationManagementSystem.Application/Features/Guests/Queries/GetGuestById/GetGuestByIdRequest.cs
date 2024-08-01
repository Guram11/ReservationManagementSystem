using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;

namespace ReservationManagementSystem.Application.Features.Guests.Queries.GetGuestById;

public sealed record GetGuestByIdRequest(Guid Id) : IRequest<GuestResponse>;
