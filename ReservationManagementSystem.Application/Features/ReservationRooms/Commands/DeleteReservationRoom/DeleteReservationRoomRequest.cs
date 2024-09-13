using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRooms.Commands.DeleteReservationRoom;

public sealed record DeleteReservationRoomRequest(Guid ReservationRoomId) : IRequest<Result<ReservationRoomResponse>>;
