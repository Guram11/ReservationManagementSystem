using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRoomPayment.Common;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.ReservationRoomPayment.Commands.CreateReservationRoomPayment;

public sealed record CreateReservationRoomPaymentRequest(Guid ReservationRoomId, decimal Amount, string Description,
    Currencies Currency) : IRequest<Result<ReservationRoomPaymentsResponse>>;
