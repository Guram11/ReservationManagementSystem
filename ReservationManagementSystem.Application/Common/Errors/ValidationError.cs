using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Common.Errors;

public static class ValidationError
{
    public static Error ValidationFailed(string message) => new Error(
        ErrorType.ValidationError, $"Validation error. {message}");
}
