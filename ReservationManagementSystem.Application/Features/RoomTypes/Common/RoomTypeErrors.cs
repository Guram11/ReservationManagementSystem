using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Common;

public static class RoomTypeErrors
{
    public static Error NotFound(Guid id) => new Error(
         ErrorType.NotFoundError, $"RoomType with ID {id} was not found.");
}
