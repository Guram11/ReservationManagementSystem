using MediatR;
using ReservationManagementSystem.Application.Features.RateTimelines.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RateTimelines.PushPrice;

public sealed record PushPriceRequest(Guid RateId, Guid RoomTypeId, DateTime StartDate,
    DateTime EndDate, decimal Price) : IRequest<Result<RateTimelineResponse>>;
