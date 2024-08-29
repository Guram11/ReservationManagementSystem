using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rooms.Common;

public static class RoomErrors
{
    public static Error NotFound(Guid id) => new Error(
    ErrorType.NotFoundError, $"Room with ID {id} was not found.");
}
