using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRoomService.Common;

public static class ReservationRoomServiceErrors
{
    public static Error NotFound() => new Error(
        "NotFound", $"ReservationRoomService was not found.");
}
