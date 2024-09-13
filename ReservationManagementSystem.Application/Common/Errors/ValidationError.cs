using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Common.Errors;

public static class ValidationError
{
    public static Error ValidationFailed(string message) => new Error(
        ErrorType.ValidationError, $"Validation error. {message}");

    public static Error ResourceInUse() => new Error(
        ErrorType.ResourceInUse, $"Can't delete this object, as it's used by other resources!");
}
