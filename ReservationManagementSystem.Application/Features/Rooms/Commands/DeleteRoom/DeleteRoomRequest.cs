using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.DeleteRoom;

public sealed record DeleteRoomRequest(Guid Id) : IRequest<RoomResponse>;