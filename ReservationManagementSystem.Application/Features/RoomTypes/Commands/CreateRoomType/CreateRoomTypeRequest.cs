using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.CreateRoomType;

public sealed record CreateRoomTypeRequest(Guid HotelId, string Name, byte NumberOfRooms,
    bool IsActive, byte MinCapacity, byte MaxCapacity) : IRequest<RoomTypeResponse>;