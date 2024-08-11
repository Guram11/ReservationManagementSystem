using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.UpdateRoom;

public sealed record UpdateRoomRequest(Guid Id, Guid RoomTypeId, string Number, byte Floor, string? Note) : IRequest<Result<RoomResponse>>;
