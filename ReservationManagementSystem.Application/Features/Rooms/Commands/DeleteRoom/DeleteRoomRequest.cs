using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.DeleteRoom;

public sealed record DeleteRoomRequest(Guid Id) : IRequest<Result<RoomResponse>>;