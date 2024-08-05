using MediatR;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.CreateRateRoomType;

public sealed record CreateRateRoomTypeRequest(Guid RateId, Guid RoomTypeId) : IRequest<Result<RateRoomTypeResponse>>;
