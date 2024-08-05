using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rooms.Queries.GetRoomById;

public sealed record GetRoomByIdRequest(Guid Id) : IRequest<Result<RoomResponse>>;