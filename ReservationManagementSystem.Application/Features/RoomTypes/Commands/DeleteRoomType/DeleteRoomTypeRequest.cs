using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.DeleteRoomType;

public sealed record DeleteRoomTypeRequest(Guid Id) : IRequest<RoomTypeResponse>;