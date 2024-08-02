using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.UpdateRoomType;

public sealed record UpdateRoomTypeRequest(Guid Id, Guid HotelId, string Name, byte NumberOfRooms,
    bool IsActive, byte MinCapacity, byte MaxCapacity) : IRequest<RoomTypeResponse>;
