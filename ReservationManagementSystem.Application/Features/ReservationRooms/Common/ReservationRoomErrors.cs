using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRooms.Common;

public static class ReservationRoomErrors
{
    public static Error NotFound() => new Error(
        "NotFound", $"ReservationRoom was not found.");
}
