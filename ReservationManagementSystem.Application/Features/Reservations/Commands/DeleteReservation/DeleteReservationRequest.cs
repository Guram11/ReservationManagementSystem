using MediatR;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Reservations.Commands.DeleteReservation;

public sealed record DeleteReservationRequest(Guid Id) : IRequest<Result<ReservationResponse>>;
