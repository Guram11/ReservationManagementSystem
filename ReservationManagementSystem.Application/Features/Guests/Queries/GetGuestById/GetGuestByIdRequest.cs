using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Guests.Queries.GetGuestById;

public sealed record GetGuestByIdRequest(Guid Id) : IRequest<Result<GuestResponse>>;
