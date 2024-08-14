using MediatR;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;

public sealed record CreateReservationRequest(Guid HotelId, string Number, decimal Price, ReservationStatus StatusId,
    DateTime Checkin, DateTime Checkout, Currencies Currency) : IRequest<Result<ReservationResponse>>;
