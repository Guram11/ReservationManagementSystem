using MediatR;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;

namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.DeleteRateRoomType;

public sealed record DeleteRateRoomTypeRequest(Guid Id) : IRequest<RateRoomTypeResponse>;