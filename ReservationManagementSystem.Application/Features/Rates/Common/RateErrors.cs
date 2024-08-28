using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rates.Common;

public static class RateErrors
{
    public static Error NotFound(Guid id) => new Error(
      "NotFound", $"Rate with ID {id} was not found.");
}
