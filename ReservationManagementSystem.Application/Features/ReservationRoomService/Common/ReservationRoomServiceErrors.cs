using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRoomService.Common;

public static class ReservationRoomServiceErrors
{
    public static Error NotFound() => new Error(
        ErrorType.NotFoundError, $"ReservationRoomService was not found.");
}
