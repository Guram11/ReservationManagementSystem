using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Common.Errors;

public static class NotFoundError
{
    public static Error NotFound(string message) => new Error(
       "NotFound", $"{message}");
}
