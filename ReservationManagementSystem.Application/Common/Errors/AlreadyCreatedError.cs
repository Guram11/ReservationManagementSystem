using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Common.Errors;

public static class AlreadyCreatedError
{
    public static Error AlreadyCreated(string message) => new Error(
       "AlreadyCreated", $"{message}");
}
