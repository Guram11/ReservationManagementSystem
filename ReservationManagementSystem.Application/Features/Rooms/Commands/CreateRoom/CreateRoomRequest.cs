using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.CreateRoom;

public sealed record CreateRoomRequest(Guid RoomTypeId, string Number, int Floor, string? Note) : IRequest<Result<RoomResponse>>;
