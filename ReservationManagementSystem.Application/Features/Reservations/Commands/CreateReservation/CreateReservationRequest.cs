using MediatR;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;

public sealed record CreateReservationRequest(DateTime Checkin, DateTime Checkout, Guid HotelId, Guid RoomTypeId, Guid RateId,
    int NumberOfRooms, Currencies Currency) : IRequest<Result<ReservationResponse>>;
