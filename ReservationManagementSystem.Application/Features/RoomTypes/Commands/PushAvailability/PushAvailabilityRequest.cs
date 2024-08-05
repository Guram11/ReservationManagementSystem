using MediatR;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.PushAvailability;

public sealed record PushAvailabilityRequest(Guid RoomTypeId, DateOnly StartDate, DateOnly EndDate, byte AvailableRooms) : IRequest<Result<string>>;
