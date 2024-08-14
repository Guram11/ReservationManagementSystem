using MediatR;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.AvailibilityTimeline.PushAvailability;

public sealed record PushAvailabilityRequest(Guid RoomTypeId, DateTime StartDate,
    DateTime EndDate, byte AvailableRooms) : IRequest<Result<AvailabilityResponse>>;
