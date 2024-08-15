using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRooms.Commands.CreateReservationRoom;

public sealed record CreateReservationRoomRequest(Guid ReservationId, Guid RoomId, Guid RateId, Guid RoomTypeId, 
    DateTime Checkin, DateTime Checkout, decimal Price) : IRequest<Result<ReservationRoomResponse>>;
