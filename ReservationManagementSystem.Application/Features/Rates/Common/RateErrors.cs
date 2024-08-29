using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rates.Common;

public static class RateErrors
{
    public static Error NotFound(Guid id) => new Error(
      ErrorType.NotFoundError, $"Rate with ID {id} was not found.");
}
