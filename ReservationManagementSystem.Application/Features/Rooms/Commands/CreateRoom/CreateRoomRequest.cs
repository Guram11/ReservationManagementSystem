using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.CreateRoom;

public sealed record CreateRoomRequest(Guid RoomTypeId, string Number, byte Floor, string? Note) : IRequest<RoomResponse>;
