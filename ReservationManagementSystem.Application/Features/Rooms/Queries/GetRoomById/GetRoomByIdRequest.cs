using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;

namespace ReservationManagementSystem.Application.Features.Rooms.Queries.GetRoomById;

public sealed record GetRoomByIdRequest(Guid Id) : IRequest<RoomResponse>;