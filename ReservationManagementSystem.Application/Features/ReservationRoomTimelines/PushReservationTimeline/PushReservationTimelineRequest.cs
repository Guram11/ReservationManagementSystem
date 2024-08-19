using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRoomTimelines.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRoomTimelines.PushReservationTimeline;

public sealed record PushReservationTimelineRequest(Guid ReservationRoomId, DateTime StartDate,
    DateTime EndDate, decimal Price) : IRequest<Result<ReservationRoomTimelineResponse>>;

