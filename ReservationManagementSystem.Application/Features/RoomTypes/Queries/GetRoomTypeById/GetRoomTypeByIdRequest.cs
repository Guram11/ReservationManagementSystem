using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetRoomTypeById;

public sealed record GetRoomTypeByIdRequest(Guid Id) : IRequest<Result<RoomTypeResponse>>;
