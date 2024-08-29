using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Reservations.Common;

public static class ReservationErrors
{
    public static Error NotFound(Guid id) => new Error(
      ErrorType.NotFoundError, $"Reservation with ID {id} was not found.");

    public static Error InvalidDataPassed() => new Error(
      ErrorType.InvalidDataPassedError, "Invalid data passed, Please check room availability");
}
