using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetRoomTypeById;

public sealed record GetRoomTypeByIdRequest(Guid Id) : IRequest<RoomTypeResponse>;
