using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.UpdateRoom;

public sealed record UpdateRoomRequest(Guid Id, Guid RoomTypeId, string Number, byte Floor, string? Note) : IRequest<RoomResponse>;
