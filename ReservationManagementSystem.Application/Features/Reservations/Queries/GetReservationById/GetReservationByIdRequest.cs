using MediatR;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Reservations.Queries.GetReservationById;

public sealed record GetReservationByIdRequest(Guid Id) : IRequest<Result<ReservationResponse>>;
