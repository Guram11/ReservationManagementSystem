using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Hotels.Common;

public static class HotelValidationError
{
    public static Error ValidationFailed(string message) => new Error(
        "Hotel.ValidationFailed", $"Validation error. '{message}'.");
}
