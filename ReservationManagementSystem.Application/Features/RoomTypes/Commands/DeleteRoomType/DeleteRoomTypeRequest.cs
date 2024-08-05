using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.DeleteRoomType;

public sealed record DeleteRoomTypeRequest(Guid Id) : IRequest<Result<RoomTypeResponse>>;