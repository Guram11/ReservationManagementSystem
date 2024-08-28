using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Reservations.Common;

public static class ReservationErrors
{
    public static Error NotFound(Guid id) => new Error(
      "NotFound", $"Reservation with ID {id} was not found.");

    public static Error InvalidDataPassed() => new Error(
     "InvalidDataPassed", "Invalid data passed, Please check room availability");
}
