using MediatR;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.DeleteRateRoomType;

public sealed record DeleteRateRoomTypeRequest(Guid RateId, Guid RoomTypeId) : IRequest<Result<RateRoomTypeResponse>>;