using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.CreateRoomType;

public sealed record CreateRoomTypeRequest(Guid HotelId, string Name, byte NumberOfRooms,
    bool IsActive, byte MinCapacity, byte MaxCapacity) : IRequest<Result<RoomTypeResponse>>;