using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Common;

public static class RateRoomTypeErrors
{
    public static Error NotFound() => new Error(
        "NotFound", $"RateRoomType was not found.");
}
