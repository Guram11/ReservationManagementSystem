using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rooms.Common;

public static class RoomErrors
{
    public static Error NotFound(Guid id) => new Error(
    "NotFound", $"Room with ID {id} was not found.");
}
