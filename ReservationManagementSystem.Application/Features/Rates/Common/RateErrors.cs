using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rates.Common;

public static class RateErrors
{
    public static Error NotFound(string message) => new Error(
        "RateNotFound", $"{message}");
}
