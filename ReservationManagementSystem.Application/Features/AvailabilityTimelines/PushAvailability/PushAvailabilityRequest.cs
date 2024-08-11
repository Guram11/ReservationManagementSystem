using MediatR;
using ReservationManagementSystem.Application.Features.AvailabilityTimelines.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.AvailabilityTimelines.PushAvailability;

public sealed record PushAvailabilityRequest(Guid RoomTypeId, DateTime StartDate,
    DateTime EndDate, byte AvailableRooms) : IRequest<Result<AvailabilityResponse>>;
