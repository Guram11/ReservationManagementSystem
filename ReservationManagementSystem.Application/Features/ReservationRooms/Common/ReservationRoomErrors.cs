using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRooms.Common;

public static class ReservationRoomErrors
{
    public static Error NotFound() => new Error(
        ErrorType.NotFoundError, $"ReservationRoom was not found.");
}
